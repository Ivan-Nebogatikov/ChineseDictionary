using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class ExtendedWord : Word
    {
        public ICollection<Example> Examples { get; set; } = new List<Example>();
    }
}
