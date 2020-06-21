using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public interface IDictionaryService
    {
        IEnumerable<ExtendedWord> SearchByChinese(string chinese, int skip = 0, int take = int.MaxValue);
        IEnumerable<ExtendedWord> SearchByTranslation(string translation, int skip = 0, int take = int.MaxValue);

    }
}
