using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class Word
    {
        public long? Id { get; set; }
        public string Chinese { get; set; }

        public string Pinyin { get; set; }

        public ICollection<string> Translations { get; set; } = new List<string>();

        public ICollection<Example> Examples { get; set; } = new List<Example>();

        public ICollection<string> RelativeWords { get; set; } = new List<string>();
    }
}
