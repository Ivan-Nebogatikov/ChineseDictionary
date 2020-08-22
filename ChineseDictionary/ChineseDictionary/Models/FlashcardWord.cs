using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class FlashcardWord
    {
        public long? Id { get; set; }
        public string Chinese { get; set; }
        public int Day { get; set; }
        public long LastTrainDate { get; set; }
    }
}
