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

namespace ChineseDictionary.Services
{
    public class HttpDictionaryService : IDictionaryService
    {

        private List<ExtendedWord> dict;
        private HttpClient Http;

        private async void Load()
        {
            var response = await Http.GetAsync("https://raw.githubusercontent.com/Oleg42-prog/Lanit-CD/master/short.txt");
            var code = response.StatusCode;
            dict.AddRange(DslParser.ListParse(await response.Content.ReadAsStreamAsync()));
            // Question: Parser should be async?
        }

        public HttpDictionaryService(HttpClient Http)
        {
            this.Http = Http;
            dict = new List<ExtendedWord>
            {
                new ExtendedWord{ Chinese = "你好", Pinyin = "nǐ hǎo", Translations = new List<string>{ "привет", "здравствуй" }, Examples = new List<Example> { new Example { Chinese = "王女士，你好！", Translation = "Добрый день, госпожа Ван!" } } },
                new ExtendedWord{ Chinese = "你", Pinyin = "nǐ", Translations = new List<string>{ "ты", "твой" } },
                new ExtendedWord{ Chinese = "好", Pinyin = "hǎo", Translations = new List<string>{ "хорошо" } }
            };
            Load();
        }

        public IEnumerable<ExtendedWord> SearchByChinese(string chinese, int skip = 0, int take = int.MaxValue)
        {
            return dict.Where(x => x.Chinese.Contains(chinese)).Skip(0).Take(take);
        }

        public IEnumerable<ExtendedWord> SearchByTranslation(string translation, int skip = 0, int take = int.MaxValue)
        {
            return dict.Where(x => x.Translations.Any(y => y.Contains(translation))).Skip(0).Take(take);
        }
    }
}
