using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using TG.Blazor.IndexedDB;

namespace ChineseDictionary.Services
{
    public class DbDictionaryService : IDictionaryService
    {
        private HttpClient Http;
        private IndexedDBManager DbManager;
        private List<ExtendedWord> mockDict;

        private async void Load()
        {
            mockDict = new List<ExtendedWord>
            {
                new ExtendedWord{ Chinese = "test", Pinyin = "may", Translations = new List<string>{ "привет", "здравствуй" }, Examples = new List<Example> { new Example { Chinese = "王女士，你好！", Translation = "Добрый день, госпожа Ван!" } } },
                new ExtendedWord{ Chinese = "test", Pinyin = "may", Translations = new List<string>{ "ты", "твой" } },
                new ExtendedWord{ Chinese = "test", Pinyin = "may", Translations = new List<string>{ "хорошо" } }
            };

            var response = await Http.GetAsync("https://raw.githubusercontent.com/Oleg42-prog/Lanit-CD/master/short.txt");
            var code = response.StatusCode;
            // Question: I think that async was wrong
            DslParser.DBParseAsync(DbManager, await response.Content.ReadAsStreamAsync());
        }

        public DbDictionaryService(HttpClient Http, IndexedDBManager DbManager)
        {
            this.Http = Http;
            this.DbManager = DbManager;
            Load();
        }

        public IEnumerable<ExtendedWord> SearchByChinese(string chinese, int skip = 0, int take = int.MaxValue)
        {
            return mockDict.Where(x => x.Chinese.Contains(chinese)).Skip(0).Take(take);
        }

        public IEnumerable<ExtendedWord> SearchByTranslation(string translation, int skip = 0, int take = int.MaxValue)
        {
            return mockDict.Where(x => x.Translations.Any(y => y.Contains(translation))).Skip(0).Take(take);
        }
    }
}
