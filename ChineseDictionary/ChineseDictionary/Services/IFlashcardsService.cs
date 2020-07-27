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
        Task<FlashcardWord> GetRandomWordByGroup(int group);
        Task<bool> IsCorrectTranslation(string chinese, string translate);
    }
}
