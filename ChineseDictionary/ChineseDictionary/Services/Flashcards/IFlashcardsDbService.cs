using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    public interface IFlashcardsDbService
    {
        Task<TrainItem[]> GenerateTrainOptions(int group, int count);
        Task<TrainItem[]> GenerateTrainReview(int group, int count);
        
        Task MoveFlashcards(IEnumerable<Word> words, int group);
        Task MoveFlashcards(IEnumerable<FlashcardWord> flashcardWords, int group);
        Task MoveFlashcard(FlashcardWord flashcardWord, int group);
        
        Task AddWord(Word word);
        Task<bool> IsWordInFlashcards(Word word);
        Task<string> NextTrainDate(Word word);
    }
}
