using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    interface IFlashcardsStatusService
    {
        public void BeginTrainReview(TrainItem[] trainItems, int group, int wordsCount);
        public void BeginTrainOptions(TrainItem[] trainItems, int group, int wordsCount);
        
        public void Answer(string translate);
        public void Answer(bool remember);

        public void StopTrain();
        public void Restore();

        public bool IsStateBegin();
        public bool IsStateTrainingOptions();
        public bool IsStateTrainingReview();
        public bool IsStateEnd();

        public Word GetWord();
        public string[] GetTranslationOptions();
        public List<Word> GetCorrect();
        public List<Word> GetWrong();
        public List<string> GetAnswers();
        public int GetGroup();
    }
}
