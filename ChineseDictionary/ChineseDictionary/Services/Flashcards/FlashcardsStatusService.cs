using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ChineseDictionary.Services
{
    public enum TrainState
    {
        Begin,
        Training,
        End
    }

    public enum TrainType
    {
        Options,
        Review
    }
    public class FlashcardsStatusService : IFlashcardsStatusService
    {
        private int index = 0;

        private Word[] questions;
        private string[][] translations;

        private List<Word> correct;
        private List<Word> wrong;
        private List<string> answers;

        private TrainState state;
        private TrainType type = TrainType.Options;

        private int group;
        private int wordsCount;

        private NavigationManager navigation;

        public FlashcardsStatusService(NavigationManager navigation)
        {
            state = TrainState.Begin;
            this.navigation = navigation;
        }

        #region BeginTrains
        private void BeginTrain(Word[] questions, int group, int wordsCount, TrainType type)
        {
            this.questions = questions;
            this.group = group;
            this.wordsCount = wordsCount;
            this.type = type;

            index = 0;
            correct = new List<Word>();
            wrong = new List<Word>();
            answers = new List<string>();

            state = TrainState.Training;
        }

        public void BeginTrainReview(Word[] questions, int group, int wordsCount)
        {
            BeginTrain(questions, group, wordsCount, TrainType.Review);
        }

        public void BeginTrainOptions(Word[] questions, string[][] translations, int group, int wordsCount)
        {
            this.translations = translations;
            BeginTrain(questions, group, wordsCount, TrainType.Options);
        }
        #endregion

        #region Answers
        public void AnswerOptions(string translate)
        {
            answers.Add(translate);
            Answer(questions[index].Translations.Contains(translate));
        }

        public void AnswerReview(bool remember)
        {
            Answer(remember);
        }

        private void Answer(bool isCorrect)
        {
            if (isCorrect)
                correct.Add(questions[index]);
            else
                wrong.Add(questions[index]);

            index++;

            if (index >= wordsCount)
                StopTrain();
        }
        #endregion

        public void StopTrain()
        {
            state = TrainState.End;
            navigation.NavigateTo("flashcards/results"); // Maybe it's wrong way & redirect need to be moved to a razor file
        }

        public void Restore()
        {
            state = TrainState.Begin;
        }

        #region State & type predicates
        // Maybe functions need to be replaced with properties
        public bool IsStateBegin()
        {
            return state == TrainState.Begin;
        }

        public bool IsStateTrainingOptions()
        {
            return state == TrainState.Training && type ==TrainType.Options;
        }

        public bool IsStateTrainingReview()
        {
            return state == TrainState.Training && type == TrainType.Review;
        }

        public bool IsStateEnd()
        {
            return state == TrainState.End;
        }

        #endregion

        #region Getters

        public Word GetWord()
        {
            if (index < questions.Length)
                return questions[index];
            else
                return null;
        }

        public string[] GetTranslations()
        {
            if (index < translations.Length)
                return translations[index];
            else
                return null;
        }

        public List<Word> GetCorrect()
        {
            return correct;
        }
        public List<Word> GetWrong()
        {
            return wrong;
        }
        public List<string> GetAnswers()
        {
            return answers;
        }

        public int GetGroup()
        {
            return group;
        }

        #endregion
    }
}
