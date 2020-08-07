using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class TrainItem
    {
        public Word Question { private set; get; }
        public string Answer { private set; get; }

        public string[] TranslationOptions { private set; get; }
        
        public bool IsCorrect { private set; get; }

        public TrainItem(Word question)
        {
            this.Question = question;
        }

        public TrainItem(Word question, string[] translationOptions)
        {
            this.Question = question;
            this.TranslationOptions = translationOptions;
        }

        public void GiveAnswer(string answer)
        {
            if (TranslationOptions is null)
                return;

            this.Answer = answer;
            this.IsCorrect = Question.Translations.Contains(answer);
        }

        public void GiveAnswer(bool correct)
        {
            this.IsCorrect = correct;
        }
    }
}
