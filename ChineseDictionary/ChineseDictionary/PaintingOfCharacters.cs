using ChineseDictionary.Constants;
using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ChineseDictionary
{
    public class PaintingOfCharacters
    {
        public static char[] firstTone = new char[] { 'Ā', 'ā', 'Ē', 'ē', 'Ī', 'ī', 'Ō', 'ō', 'Ū', 'ū', 'Ǖ', 'ǖ' };
        public static char[] secondTone = new char[] { 'Á', 'á', 'É', 'é', 'Í', 'í', 'Ó', 'ó', 'Ú', 'ú', 'Ǘ', 'ǘ' };
        public static char[] thirdTone = new char[] { 'Ǎ', 'ǎ', 'Ě', 'ě', 'Ǐ', 'ǐ', 'Ǒ', 'ǒ', 'Ǔ', 'ǔ', 'Ǚ', 'ǚ' };
        public static char[] fourthTone = new char[] { 'À', 'à', 'È', 'è', 'Ì', 'ì', 'Ò', 'ò', 'Ù', 'ù', 'Ǜ', 'ǜ' };
        public static char[] fifthTone = new char[] { 'A', 'a', 'E', 'e', 'I', 'i', 'O', 'o', 'U', 'u', 'Ü', 'ü' };
        public static char[] separators = new char[] { 'b', 'p', 'm', 'f', 'd', 't', 'n', 'l', 'g', 'k', 'h', 'j', 'q', 'x', 'z', 'c', 's', 'r', 'y' };

        public static List<ColoredCharacter> PaintCharacter(string word, string chinese)
        {
            string[] split = word.Split(separators);

            int j = 0;
            
            List<ColoredCharacter> coloredCharactersList = new List<ColoredCharacter> { };

            foreach (string splitSymbols in split)
            {
                if (!string.IsNullOrWhiteSpace(splitSymbols))
                {
                    string style = ColorConstants.fifthTone;
                    foreach (char symbol in splitSymbols)
                    {
                        if (firstTone.Contains(symbol))
                            style = ColorConstants.firstTone;
                        if (secondTone.Contains(symbol))
                            style = ColorConstants.secondTone;
                        if (thirdTone.Contains(symbol))
                            style = ColorConstants.thirdTone;
                        if (fourthTone.Contains(symbol))
                            style = ColorConstants.fourthTone;
                    }

                    ColoredCharacter coloredCharacter = new ColoredCharacter(chinese[j], style);
                    coloredCharactersList.Add(coloredCharacter);
                    j++;
                }
            }
            return coloredCharactersList;
        }
    }
}
