using System;
using System.Collections.Generic;
using System.Text;

namespace HandwritingLibrary
{
    public class CharConstants
    {
        // magic constants
        public const int MAX_CHARACTER_STROKE_COUNT = 48;
        public const int MAX_CHARACTER_SUB_STROKE_COUNT = 64;
        public const double DEFAULT_LOOSENESS = 0.15;
        public const double AVG_SUBSTROKE_LENGTH = 0.33; // an average length (out of 1)
        public const double SKIP_PENALTY_MULTIPLIER = 1.75; // penalty mulitplier for skipping a stroke
        public static double CORRECT_NUM_STROKES_BONUS = 0.1; // max multiplier bonus if characters has the correct number of strokes
        public static int CORRECT_NUM_STROKES_CAP = 10;  // characters with more strokes than this will not be multiplied


        // the actual Character.
        public char Character;



        // one of CharacterTypeRepository types (traditional, simplified, etc).
        //public int CharacterType;

        public int StrokeCount;	// number of strokes
        public int SubStrokeCount; // number of "substrokes"

        // the directions and lengths of each substroke.
        // indexed by substroke index - 1
        private double[] directions = new double[MAX_CHARACTER_SUB_STROKE_COUNT];
        private double[] lengths = new double[MAX_CHARACTER_SUB_STROKE_COUNT];

        public double[] Directions
        {
            get
            {
                return directions;
            }
        }

        public double[] Lengths
        {
            get
            {
                return lengths;
            }
        }
    }
}
