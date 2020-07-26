using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Handwriting
{
    class Matcher
    {
        public class StrokesMatcher
        {
            private CharConstants inputCharacter;
            private CharConstants compareTo;


            private double[][] res;


            private double looseness;

            private MatchCollector matchCollector;

            private bool running;


            public StrokesMatcher(CharConstants character,
                                    double looseness,
                                    int numMatches)
            {

                this.inputCharacter = character;
                this.compareTo = new CharConstants();


                this.looseness = looseness;

                this.running = true;

                this.matchCollector = new MatchCollector(numMatches);
                this.buildScoreMatrix();
            }

            public char[] DoMatching()
            {
                int strokeCount = inputCharacter.StrokeCount;
                int subStrokeCount = inputCharacter.SubStrokeCount;


                int strokeRange = getStrokesRange(strokeCount);
                int minimumStrokes = Math.Max(strokeCount - strokeRange, 1);
                int maximumStrokes = Math.Min(strokeCount + strokeRange, CharConstants.MAX_CHARACTER_STROKE_COUNT);

                int subStrokesRange = getSubStrokesRange(subStrokeCount);
                var minSubStrokes = Math.Max(subStrokeCount - subStrokesRange, 1);
                var maxSubStrokes = Math.Min(subStrokeCount + subStrokesRange, CharConstants.MAX_CHARACTER_SUB_STROKE_COUNT);


                JArray jarrayObj = new JArray();
                using (StreamReader reader = File.OpenText("D:/Projects/CHINESE DICTIONARY/HanziLookupJS/dist/orig.json"))
                {

                    JObject chars = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                    var symbols = chars["chars"] as JArray;


                    for (int i = 0; i <= symbols.Count - 1; i++)
                    {
                        var repoChar = symbols[i] as JArray;
                        int cmpStrokeCount = repoChar[1].ToObject<int>();
                        int cmpSubStrokes = repoChar[2].ToObject<int>();
                        if (cmpStrokeCount < minimumStrokes || cmpStrokeCount > maximumStrokes) continue;
                        if (cmpSubStrokes < minSubStrokes || cmpSubStrokes > maxSubStrokes) continue;
                        var match = matchOne(strokeCount, inputSubStrokes, subStrokesRange, repoChar);
                        this.matchCollector.fileMatch(match);

                    }


                }


                char[] matches = this.matchCollector.getMatches();

                if (running)
                {
                    return matches;
                }

                return null;
            }

            private int getStrokesRange(int strokeCount)
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

                double[] solutions = new double[1];
                CubicCurve2D curve = new CubicCurve2D(0, 0, ctrl1X, ctrl1Y, ctrl2X, ctrl2Y, 1, CharConstants.MAX_CHARACTER_STROKE_COUNT);
                double t = curve.GetFirstSolutionForX(looseness);

                return (int)Math.Round(curve.GetYOnCurve(t));
            }

            private int getSubStrokesRange(int subStrokeCount)
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

                double[] solutions = new double[1];
                CubicCurve2D curve = new CubicCurve2D(0, y0, ctrl1X, ctrl1Y, ctrl2X, ctrl2Y, 1, CharConstants.MAX_CHARACTER_SUB_STROKE_COUNT);
                double t = curve.GetFirstSolutionForX(looseness);
                return (int)Math.Round(curve.GetYOnCurve(t));
            }


            private void buildScoreMatrix()
            {
                double AVG_SUBSTROKE_LENGTH = 0.33;
                static private double SKIP_PENALTY_MULTIPLIER = 1.75;

                int dim = CharConstants.MAX_CHARACTER_SUB_STROKE_COUNT + 1;
                this.res = new double[dim][];
                for (int i = 0; i != res.Length; ++i)
                    res[i] = new double[dim];
                for (int i = 0; i < dim; i++)
                {
                    double penalty = -AVG_SUBSTROKE_LENGTH * SKIP_PENALTY_MULTIPLIER * i;
                    this.res[i][0] = penalty;
                    this.res[0][i] = penalty;
                }

            }

            static private double[] initCubicCurveScoreTable(CubicCurve2D curve, int numSamples)
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
}
