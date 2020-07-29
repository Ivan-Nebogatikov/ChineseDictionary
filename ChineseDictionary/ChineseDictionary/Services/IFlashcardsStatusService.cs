using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    interface IFlashcardsStatusService
    {
        public Task BeginTrain(IFlashcardsDbService FlashcardsDb, int group, int wordsCount);

        public Task Answer(IFlashcardsDbService FlashcardsDb, string translate);

        public void StopTrain(IFlashcardsDbService FlashcardsDb);
        public void Restore();

        public FlashcardWord GetWord();
        public List<string> GetTranslations();

        public List<FlashcardWord> GetCorrect();
        public List<FlashcardWord> GetWrong();
        public List<string> GetAnswers();
    }
}
