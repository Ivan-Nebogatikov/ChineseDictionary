using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Services
{
    interface IFlashcardsStatusService
    {
        public void BeginTrainReview(Word[] questions, int group, int wordsCount);
        public void BeginTrainOptions(Word[] questions, string[][] translations, int group, int wordsCount);
        
        public void AnswerOptions(string translate);
        public void AnswerReview(bool remember);

        public void StopTrain();
        public void Restore();

        public bool IsStateBegin();
        public bool IsStateTrainingOptions();
        public bool IsStateTrainingReview();
        public bool IsStateEnd();

        public Word GetWord();
        public string[] GetTranslations();
        public List<Word> GetCorrect();
        public List<Word> GetWrong();
        public List<string> GetAnswers();
        public int GetGroup();
    }
}
