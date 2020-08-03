using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public interface IFlashcardsDbService
    {
        Task<Word[]> GetRandomWordsByGroup(int group, int count);
        Task<string[][]> GetRandomTranslationMatrix(Word[] questionWord, int count);
        Task MoveFlashcards(IEnumerable<Word> words, int group);
        Task MoveFlashcards(IEnumerable<FlashcardWord> flashcardWords, int group);
        Task MoveFlashcard(FlashcardWord flashcardWord, int group);
    }
}
