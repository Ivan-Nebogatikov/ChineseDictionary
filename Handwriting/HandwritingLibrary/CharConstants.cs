namespace HandwritingLibrary
{
    public class CharConstants
    {
        public const int MAX_CHARACTER_STROKE_COUNT = 48;
        public const int MAX_CHARACTER_SUB_STROKE_COUNT = 64;
        public const double DEFAULT_LOOSENESS = 0.15;
        public const double AVG_SUBSTROKE_LENGTH = 0.33; 
        public const double SKIP_PENALTY_MULTIPLIER = 1.75; 
        public static double CORRECT_NUM_STROKES_BONUS = 0.1; 
        public static int CORRECT_NUM_STROKES_CAP = 10; 
    }
}
