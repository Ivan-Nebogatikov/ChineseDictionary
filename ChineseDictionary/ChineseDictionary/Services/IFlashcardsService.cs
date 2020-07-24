using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public interface IFlashcardsService
    {
        List<string> GetRandomTranslations(string word, int count);
        Task<ExtendedWord> GetRandomWordByGroup(int group);

    }
}
