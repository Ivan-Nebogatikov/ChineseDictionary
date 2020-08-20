using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using TG.Blazor.IndexedDB;
using ChineseDictionary.Constants;

namespace ChineseDictionary.Services
{
    public class FlashcardsDbService : IFlashcardsDbService
    {
        private IndexedDBManager DbManager;
        private Random rand;

        public FlashcardsDbService(IndexedDBManager DbManager)
        {
            this.DbManager = DbManager;
            this.rand = new Random();
        }

        #region FlashcardWord & Word link
        private async Task<FlashcardWord> GetFlashcardWordByWord(Word word)
        {
            var query = new StoreIndexQuery<string>
            {
                Storename = DbConstants.StoreName,
                IndexName = DbConstants.Chinese,
                QueryValue = word.Chinese
            };

            return await DbManager.GetRecordByIndex<string, FlashcardWord>(query);
        }
        private async Task<Word> GetWordByFlashcardWord(FlashcardWord flashcardWord)
        {
            var query = new StoreIndexQuery<string>
            {
                Storename = DbConstants.StoreName,
                IndexName = DbConstants.Chinese,
                QueryValue = flashcardWord.Chinese
            };

            return await DbManager.GetRecordByIndex<string, Word>(query);
        }
        #endregion

        #region Get Random Words
        private async Task<Word> GetRandomWordByGroup(int group)
        {
            var query = new StoreIndexQuery<int>
            {
                Storename = DbConstants.FlashcardsStoreName,
                IndexName = DbConstants.Day,
                QueryValue = group
            };

            var flashcardWords = await DbManager.GetAllRecordsByIndex<int, FlashcardWord>(query);
            FlashcardWord flashcardWord = flashcardWords[rand.Next(flashcardWords.Count)];
            return await GetWordByFlashcardWord(flashcardWord);
        }
        #endregion

        #region Get Random Translations
        private async Task<string[]> GetRandomTranslations(Word questionWord, int count)
        {
            Word word;
            List<Word> words = await DbManager.GetRecords<Word>(DbConstants.StoreName);
            List<string> randomTranslations = new List<string>();
            string[] wordTranslations;
            string translation;

            while (randomTranslations.Count < count - 1)
            {
                word = words[rand.Next(words.Count)];
                wordTranslations = word.Translations.ToArray<string>();
                translation = wordTranslations[rand.Next(wordTranslations.Length)];

                if (randomTranslations.Contains(translation))
                    continue;

                randomTranslations.Add(translation);
            }

            string[] rightTranslations = questionWord.Translations.ToArray();
            string rightTranslation = rightTranslations[rand.Next(rightTranslations.Length)];
            randomTranslations.Insert(rand.Next(randomTranslations.Count), rightTranslation);

            return randomTranslations.ToArray();
        }
        #endregion

        #region Generate Train
        public async Task<TrainItem[]> GenerateTrainOptions(int group, int count)
        {
            TrainItem[] data = new TrainItem[count];

            Word word;
            string[] translationOptions;
            for(int i = 0; i < count; i ++)
            {
                word = await GetRandomWordByGroup(group);
                translationOptions = await GetRandomTranslations(word, ConfigConstants.FlashcardsOptionsCount);
                data[i] = new TrainItem(word, translationOptions);
            }

            return data;
        }

        public async Task<TrainItem[]> GenerateTrainReview(int group, int count)
        {
            TrainItem[] data = new TrainItem[count];

            Word word;
            for (int i = 0; i < count; i++)
            {
                word = await GetRandomWordByGroup(group);
                data[i] = new TrainItem(word);
            }

            return data;
        }

        public async Task<TrainItem[]> GenerateTrainWritting(int group, int count)
        {
            TrainItem[] data = new TrainItem[count];

            Word word;
            for (int i = 0; i < count; i++)
            {
                word = await GetRandomWordByGroup(group);
                data[i] = new TrainItem(word);
            }

            return data;
        }
        #endregion

        #region MoveFlashcards
        public async Task MoveFlashcards(IEnumerable<Word> words, int group)
        {
            FlashcardWord flashcardWord;
            foreach (Word word in words)
            {
                flashcardWord = await GetFlashcardWordByWord(word);
                await MoveFlashcard(flashcardWord, group);
            }
        }

        public async Task MoveFlashcards(IEnumerable<FlashcardWord> flashcardWords, int group)
        {
            foreach (FlashcardWord flashcardWord in flashcardWords)
                await MoveFlashcard(flashcardWord, group);
        }

        public async Task MoveFlashcard(FlashcardWord flashcardWord, int group)
        {
            StoreRecord<FlashcardWord> record;
            
            flashcardWord.Day = group;
            record = new StoreRecord<FlashcardWord>
            {
                Storename = DbConstants.FlashcardsStoreName,
                Data = flashcardWord
            };

            await DbManager.UpdateRecord(record);
        }
        #endregion
    }
}
