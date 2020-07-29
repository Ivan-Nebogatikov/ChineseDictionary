using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public interface IFlashcardsDbService
    {
        Task<List<string>> GetRandomTranslations(FlashcardWord questionWord, int count);
        Task<FlashcardWord> GetRandomWordByGroup(int group);
        Task<bool> IsCorrectTranslation(string chinese, string translate);
        Task MoveFlashcards(List<FlashcardWord> words, int group);
    }
}
