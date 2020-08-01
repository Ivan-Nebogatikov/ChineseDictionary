using ChineseDictionary.Constants;
using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChineseDictionary
{
    public class PaintingOfCharacters
    {
        public static List<ColoredCharacter> PaintCharacter(string word, string chinese)
        {
            char[] firstTone = new char[] { 'Ā', 'ā', 'Ē', 'ē', 'Ī', 'ī', 'Ō', 'ō', 'Ū', 'ū', 'Ǖ', 'ǖ' };
            char[] secondTone = new char[] { 'Á', 'á', 'É', 'é', 'Í', 'í', 'Ó', 'ó', 'Ú', 'ú', 'Ǘ', 'ǘ' };
            char[] thirdTone = new char[] { 'Ǎ', 'ǎ', 'Ě', 'ě', 'Ǐ', 'ǐ', 'Ǒ', 'ǒ', 'Ǔ', 'ǔ', 'Ǚ', 'ǚ' };
            char[] fourthTone = new char[] { 'À', 'à', 'È', 'è', 'Ì', 'ì', 'Ò', 'ò', 'Ù', 'ù', 'Ǜ', 'ǜ' };
            char[] fifthTone = new char[] { 'A', 'a', 'E', 'e', 'I', 'i', 'O', 'o', 'U', 'u', 'Ü', 'ü' };

            string[] split = word.Split(new Char[] { 'b', 'p', 'm', 'f', 'd', 't', 'n', 'l', 'g', 'k', 'h', 'j', 'q', 'x', 'z', 'c', 's', 'r', 'y' });

            int i;
            int j = 0;
            string style = "";
            List<ColoredCharacter> coloredCharactersList = new List<ColoredCharacter> { };

            foreach (string splitSimbols in split)
            {
                if (splitSimbols.Trim() != "")
                {
                    i = 0;
                    foreach (char simbol in splitSimbols)
                    {
                        if (firstTone.Contains(simbol))
                            i = 1;
                        if (secondTone.Contains(simbol))
                            i = 2;
                        if (thirdTone.Contains(simbol))
                            i = 3;
                        if (fourthTone.Contains(simbol))
                            i = 4;
                    }
                    switch (i)
                    {
                        case 1:
                            style = ColorConstants.firstTone;
                            break;
                        case 2:
                            style = ColorConstants.secondTone;
                            break;
                        case 3:
                            style = ColorConstants.thirdTone;
                            break;
                        case 4:
                            style = ColorConstants.fourthTone;
                            break;
                        case 0:
                            style = ColorConstants.fifthTone;
                            break;
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
