using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


namespace HandwritingLibrary
{
    public class Matcher
    {
        int strokesCount;
        int subStrokesCount;
        double[][] scoreMatrix;

        JArray jarrayObj = new JArray();
        List<SubStroke> inputSubStrokes = new List<SubStroke>();

        static double looseness = CharConstants.DEFAULT_LOOSENESS;

        MatchCollector matchCollector;

        List<SubStroke> SubStrokes;

        JToken subStrInf;

        bool running;

        CharConstants cc = new CharConstants();

        public Matcher(int strokesCount, int subStrokesCount,  List<SubStroke> inputSubStrokes, int limit)
        {
            this.strokesCount = strokesCount;
            this.subStrokesCount = subStrokesCount;
            this.inputSubStrokes = inputSubStrokes;
            matchCollector = new MatchCollector(limit);
            this.buildScoreMatrix();
        }

        public char[] DoMatching(JObject data)
        {
            int strokeCount = strokesCount;
            int subStrokeCount = subStrokesCount;
            List<SubStroke> inputSubStrokes = this.inputSubStrokes;

            int strokeRange = getStrokesRange(strokeCount);
            int minimumStrokes = Math.Max(strokeCount - strokeRange, 1);
            int maximumStrokes = Math.Min(strokeCount + strokeRange, CharConstants.MAX_CHARACTER_STROKE_COUNT);

            int subStrokesRange = getSubStrokesRange(subStrokeCount);
            var minSubStrokes = Math.Max(subStrokeCount - subStrokesRange, 1);
            var maxSubStrokes = Math.Min(subStrokeCount + subStrokesRange, CharConstants.MAX_CHARACTER_SUB_STROKE_COUNT);

            data["substrokes"] = CompactDecoder.Decode(data["substrokes"].ToString());
            subStrInf = data["substrokes"];
            var symbols = data["chars"] as JArray;

            for (int i = 0; i <= symbols.Count - 1; i++)
            {
                var repoChar = symbols[i];
                int cmpStrokeCount = repoChar[1].ToObject<int>();
                int cmpSubStrokes = repoChar[2].ToObject<int>();
                if (cmpStrokeCount < minimumStrokes || cmpStrokeCount > maximumStrokes) continue;
                if (cmpSubStrokes < minSubStrokes || cmpSubStrokes > maxSubStrokes) continue;
                jarrayObj.Add(symbols[i]);
                CharacterMatch match = this.matchOne(strokeCount, inputSubStrokes, subStrokesRange, repoChar);
                matchCollector.AddMatch(match);
            }

            char[] resultChars = matchCollector.matches.Select(x => x.Character).ToArray();
            return resultChars;
        }

        private static int getStrokesRange(int strokeCount)
        {
            if (looseness == 0.0)
            {
                return 0;
            }
            else if (looseness == 1.0)
            {
                return CharConstants.MAX_CHARACTER_STROKE_COUNT;
            }
            double ctrl1X = 0.35;
            double ctrl1Y = strokeCount * 0.4;

            double ctrl2X = 0.6;
            double ctrl2Y = strokeCount;

            CubicCurve2D curve = new CubicCurve2D(0, 0, ctrl1X, ctrl1Y, ctrl2X, ctrl2Y, 1, CharConstants.MAX_CHARACTER_STROKE_COUNT);
            double t = curve.GetFirstSolutionForX(looseness);

            return (int)Math.Round(curve.GetYOnCurve(t));
        }

        private static int getSubStrokesRange(int subStrokeCount)
        {
            if (looseness == 1.0)
            {
                return CharConstants.MAX_CHARACTER_SUB_STROKE_COUNT;
            }
            double y0 = subStrokeCount * 0.25;

            double ctrl1X = 0.4;
            double ctrl1Y = 1.5 * y0;

            double ctrl2X = 0.75;
            double ctrl2Y = 1.5 * ctrl1Y;

            CubicCurve2D curve = new CubicCurve2D(0, y0, ctrl1X, ctrl1Y, ctrl2X, ctrl2Y, 1, CharConstants.MAX_CHARACTER_SUB_STROKE_COUNT);
            double t = curve.GetFirstSolutionForX(looseness);
            return (int)Math.Round(curve.GetYOnCurve(t));
        }
        
        private void buildScoreMatrix()
        {
            double AVG_SUBSTROKE_LENGTH = 0.33;
            double SKIP_PENALTY_MULTIPLIER = 1.75;

            int dim = CharConstants.MAX_CHARACTER_SUB_STROKE_COUNT + 1;
            this.scoreMatrix = new double[dim][];
            for (int i = 0; i != scoreMatrix.Length; ++i)
                scoreMatrix[i] = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                double penalty = -AVG_SUBSTROKE_LENGTH * SKIP_PENALTY_MULTIPLIER * i;
                scoreMatrix[i][0] = penalty;
                scoreMatrix[0][i] = penalty;
            }

        }
        double SKIP_PENALTY_MULTIPLIER = 1.75;
        
        private CharacterMatch matchOne(int inputStrokeCount, List<SubStroke> inputSubStrokes, int subStrokesRange, JToken repoChar)
        {
            double score = (double) this.computeMatchScore(inputStrokeCount, inputSubStrokes, subStrokesRange, repoChar);

            if (inputStrokeCount == repoChar[1].ToObject<int>() && inputStrokeCount < CharConstants.CORRECT_NUM_STROKES_CAP)
            {
                double bonus = CharConstants.CORRECT_NUM_STROKES_BONUS * ((double)(Math.Max(CharConstants.CORRECT_NUM_STROKES_CAP - inputStrokeCount, 0) / CharConstants.CORRECT_NUM_STROKES_CAP));
                score += bonus * score;
            }
            return new CharacterMatch(repoChar[0].ToObject<char>(), score);
        }
        private object computeMatchScore(int strokeCount, List<SubStroke> inputSubStrokes, int subStrokesRange, JToken repoChar)
        {
           for (int x = 0; x < (inputSubStrokes.Count); x++)
            {
                int inputDirection = inputSubStrokes[x].Dir;
                double inputLength = inputSubStrokes[x].Len;
                
                for (int y = 0; y < repoChar[2].ToObject<int>(); y++)
                {
                    double newScore = Double.NegativeInfinity;

                    if (Math.Abs(x - y) <= subStrokesRange)
                    {
                        var compareDirection = subStrInf[repoChar[3].ToObject<int>() + y * 3];
                        var compareLength = (subStrInf[repoChar[3].ToObject<int>() + y * 3 + 1]);
                        
                        double skip1Score = scoreMatrix[x][y + 1] - (inputLength / 256 * SKIP_PENALTY_MULTIPLIER);
                        double skip2Score = scoreMatrix[x + 1][y] - ((double)compareLength * SKIP_PENALTY_MULTIPLIER);
                        double skipScore = Math.Max(skip1Score, skip2Score);
                        double matchScore = computeSubStrokeScore(inputDirection, inputLength, compareDirection.ToObject<int>(), compareLength.ToObject<double>());
                        double previousScore = this.scoreMatrix[x][y];
                        newScore = Math.Max(previousScore + matchScore, skipScore);
                    }
                    this.scoreMatrix[x + 1][y + 1] = newScore;
                }
            }
            return this.scoreMatrix[inputSubStrokes.Count][repoChar[2].ToObject<int>()];
        }

        private double computeSubStrokeScore(double inputDir, double inputLen, double repoDir, double repoLen)
        {
            double directionScore = getDirectionScore(inputDir, repoDir, inputLen);
            double lengthScore = getLengthScore(inputLen, repoLen);
            
            double score = lengthScore * directionScore;
            return score;
        }

        private static double[] DIRECTION_SCORE_TABLE = initDirectionScoreTable();
        private static double[] LENGTH_SCORE_TABLE = initLengthScoreTable();

        private static double[] initDirectionScoreTable()
        {
            CubicCurve2D curve = new CubicCurve2D(0, 1.0, 0.5, 1.0, 0.25, -2.0, 1.0, 1.0);
            return initCubicCurveScoreTable(curve, 256);
        }

        private static double[] initLengthScoreTable()
        {
            CubicCurve2D curve = new CubicCurve2D(0, 0, 0.25, 1.0, 0.75, 1.0, 1.0, 1.0);
            return initCubicCurveScoreTable(curve, 129);
        }

        private static double[] initCubicCurveScoreTable(CubicCurve2D curve, int numSamples)
        {
            double x1 = curve.X1;
            double x2 = curve.X2;

            double range = x2 - x1;

            double x = x1;
            double xInc = range / numSamples;  
            double[] scoreTable = new double[numSamples];
            double[] solutions = new double[1];

            for (int i = 0; i < numSamples; i++)
            {
                double t = curve.GetFirstSolutionForX(Math.Min(x, x2));
                scoreTable[i] = curve.GetYOnCurve(t);
                x += xInc;
            }
            return scoreTable;
        }

        private double getDirectionScore(double direction1, double direction2, double inputLength)
        {
            double theta = Math.Abs(direction1 - direction2);
            double directionScore = DIRECTION_SCORE_TABLE[(int)theta];

            if (inputLength < 64)
            {
                double shortLengthBonusMax = Math.Min(1.0, 1.0 - directionScore);
                double shortLengthBonus = shortLengthBonusMax * (1 - (inputLength / 64));
                directionScore += shortLengthBonus;
            }
            return directionScore;
        }

        private double getLengthScore(double length1, double length2)
        {
            int ratio;
            double result;
            if (length1 > length2)
            {
                result = (((int)length2) << 7) / length1;
                ratio = (int)Math.Round(result);
            }
            else
            {
                result = (((int)length1) << 7) / length2;
                ratio = (int)Math.Round(result);
            }
            return LENGTH_SCORE_TABLE[ratio];
        }

        public void Stop()
        {
            this.running = false;
        }

        public bool IsRunning
        {
            get { return running; }
        }

    }
}
