using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public interface IDictionaryServiceAsync
    {
        Task<IEnumerable<ExtendedWord>> SearchByChineseAsync(string chinese, int skip = 0, int take = int.MaxValue);
        Task<IEnumerable<ExtendedWord>> SearchByTranslationAsync(string translation, int skip = 0, int take = int.MaxValue);
        public Task<ExtendedWord> GetByChinese(string chinese);
    }
}
