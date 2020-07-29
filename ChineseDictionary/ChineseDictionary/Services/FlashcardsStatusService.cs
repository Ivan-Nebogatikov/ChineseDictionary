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

        private List<FlashcardWord> correct;
        private List<FlashcardWord> wrong;
        private List<string> answers;

        private TrainState state;
        private TrainType type = TrainType.Options;

        private FlashcardWord word;

        private List<string> optionTranslations;
        private int group;
        private int wordsCount;

        private NavigationManager navigation;

        public FlashcardsStatusService(NavigationManager navigation)
        {
            state = TrainState.Begin;
            this.navigation = navigation;
        }

        public async Task BeginTrain(IFlashcardsDbService FlashcardsDb, int group, int wordsCount, TrainType type)
        {
            if (state == TrainState.Begin)
            {
                index = 0;
                correct = new List<FlashcardWord>();
                wrong = new List<FlashcardWord>();
                answers = new List<string>();
                this.group = group;
                this.wordsCount = wordsCount;

                state = TrainState.Training;
                word = await FlashcardsDb.GetRandomWordByGroup(group);
                
                this.type = type;
                if (type == TrainType.Options)
                    optionTranslations = await FlashcardsDb.GetRandomTranslations(word, 4);
            }

            if (state == TrainState.Training && type == TrainType.Options)
                navigation.NavigateTo("/flashcards/options"); // Maybe it's wrong way & redirect need to be moved to a razor file

            if (state == TrainState.Training && type == TrainType.Review)
                navigation.NavigateTo("/flashcards/review"); // Maybe it's wrong way & redirect need to be moved to a razor file

            if (state == TrainState.End)
                navigation.NavigateTo("/flashcards/results"); // Maybe it's wrong way & redirect need to be moved to a razor file
        }

        public async Task AnswerOptions(IFlashcardsDbService FlashcardsDb, string translate)
        {
            if (await FlashcardsDb.IsCorrectTranslation(word.Chinese, translate))
                correct.Add(word);
            else
                wrong.Add(word);

            answers.Add(translate);

            word = await FlashcardsDb.GetRandomWordByGroup(group);
            optionTranslations = await FlashcardsDb.GetRandomTranslations(word, 4);

            index++;

            if (index >= wordsCount)
                StopTrain(FlashcardsDb);
        }

        public async Task AnswerReview(IFlashcardsDbService FlashcardsDb, bool remember)
        {
            if (remember)
                correct.Add(word);
            else
                wrong.Add(word);

            word = await FlashcardsDb.GetRandomWordByGroup(group);

            index++;

            if (index >= wordsCount)
                StopTrain(FlashcardsDb);
        }

        public void StopTrain(IFlashcardsDbService FlashcardsDb)
        {
            if (state != TrainState.End)
            {
                FlashcardsDb.MoveFlashcards(correct, group + 1);
                state = TrainState.End;
                navigation.NavigateTo("/flashcards/results"); // Maybe it's wrong way & redirect need to be moved to a razor file
            }
        }

        public void Restore()
        {
            state = TrainState.Begin;
            navigation.NavigateTo("/flashcards"); // Maybe it's wrong way & redirect need to be moved to a razor file
        }

        public FlashcardWord GetWord()
        {
            return word;
        }

        public List<string> GetTranslations()
        {
            return optionTranslations;
        }

        public List<FlashcardWord> GetCorrect()
        {
            return correct;
        }
        public List<FlashcardWord> GetWrong()
        {
            return wrong;
        }
        public List<string> GetAnswers()
        {
            return answers;
        }
    }
}
