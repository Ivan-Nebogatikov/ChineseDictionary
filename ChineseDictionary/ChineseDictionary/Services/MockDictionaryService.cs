using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public class MockDictionaryService : IDictionaryService
    {

        private List<ExtendedWord> mockDict;

        public MockDictionaryService()
        {
            mockDict = new List<ExtendedWord>
            {
                new ExtendedWord{ 
                    Chinese = "你好", Pinyin = "nǐ hǎo", Translations = new List<string>{ "привет", "здравствуй" }, 
                    Examples = new List<Example> { new Example { Chinese = "王女士，你好！", Translation = "Добрый день, госпожа Ван!" } } , 
                    },
                new ExtendedWord{ Chinese = "你", Pinyin = "nǐ", Translations = new List<string>{ "ты", "твой" } },
                new ExtendedWord{ Chinese = "好", Pinyin = "hǎo", Translations = new List<string>{ "хорошо" } }
            };

            for (int i = 0; i < 100; ++i)
            {
                mockDict.Add(mockDict[0]);
            }
        }

        public IEnumerable<ExtendedWord> SearchByChinese(string chinese, int skip = 0, int take = int.MaxValue)
        {
            return mockDict.Where(x => x.Chinese.Contains(chinese)).Skip(0).Take(take);
        }

        public IEnumerable<ExtendedWord> SearchByTranslation(string translation, int skip = 0, int take = int.MaxValue)
        {
            return mockDict.Where(x => x.Translations.Any(y => y.Contains(translation))).Skip(0).Take(take);
        }

        public ExtendedWord GetByChinese(string chinese)
        {
            // ToDo: возможны проблемы со скоростью поиска
            foreach (ExtendedWord word in mockDict)
                if (word.Chinese == chinese)
                    return word;
            return new ExtendedWord() { Chinese = "error" };
        }
    }
}
