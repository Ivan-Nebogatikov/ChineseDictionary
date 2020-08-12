using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class ColoredCharacter
    {
        public string color;
        public char character;

        public ColoredCharacter(char character, string color)
        {
            this.character = character;
            this.color = color;

        }
    }
}
