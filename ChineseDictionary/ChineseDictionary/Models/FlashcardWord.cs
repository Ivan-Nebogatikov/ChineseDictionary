using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class FlashcardWord : Word
    {
        public string Chinese { get; set; }
        public int day;
    }
}
