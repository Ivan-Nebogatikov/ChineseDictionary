using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public interface IDictionaryServiceAsync
    {
        Task<IEnumerable<Word>> SearchAsync(string query, int skip = 0, int take = int.MaxValue);
        public Task<Word> GetByChinese(string chinese);
    }
}
