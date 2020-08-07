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

        private TrainItem[] trainItems;

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
        private void BeginTrain(TrainItem[] trainItems, int group, int wordsCount, TrainType type)
        {
            this.trainItems = trainItems;
            this.group = group;
            this.wordsCount = wordsCount;
            this.type = type;

            index = 0;
            state = TrainState.Training;
        }

        public void BeginTrainReview(TrainItem[] trainItems, int group, int wordsCount)
        {
            BeginTrain(trainItems, group, wordsCount, TrainType.Review);
        }

        public void BeginTrainOptions(TrainItem[] trainItems, int group, int wordsCount)
        {
            BeginTrain(trainItems, group, wordsCount, TrainType.Options);
        }
        #endregion

        #region Answers
        public void Answer(string translate)
        {
            trainItems[index].GiveAnswer(translate);
            IndexIncrement();
        }

        public void Answer(bool remember)
        {
            trainItems[index].GiveAnswer(remember);
            IndexIncrement();
        }

        private void IndexIncrement()
        {
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
            if (index < trainItems.Length)
                return trainItems[index].Question;
            else
                return null;
        }

        public string[] GetTranslationOptions()
        {
            if (index < trainItems.Length)
                return trainItems[index].TranslationOptions;
            else
                return null;
        }

        public List<Word> GetCorrect()
        {
            return trainItems.Where(item => item.IsCorrect).Select(item => item.Question).ToList();
        }
        public List<Word> GetWrong()
        {
            return trainItems.Where(item => !item.IsCorrect).Select(item => item.Question).ToList();
        }
        public List<string> GetAnswers()
        {
            return trainItems.Select(item => item.Answer).ToList();
        }

        public int GetGroup()
        {
            return group;
        }

        #endregion
    }
}
