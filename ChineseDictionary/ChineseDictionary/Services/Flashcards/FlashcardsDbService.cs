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
            flashcardWord.LastTrainDate = DateTime.Now.Ticks;
            record = new StoreRecord<FlashcardWord>
            {
                Storename = DbConstants.FlashcardsStoreName,
                Data = flashcardWord
            };

            await DbManager.UpdateRecord(record);
        }
        #endregion

        private async Task AddRecord(Word word)
        {
            await DbManager.AddRecord(new StoreRecord<FlashcardWord>
            {
                Storename = DbConstants.FlashcardsStoreName,
                Data = new FlashcardWord { Chinese = word.Chinese, Day = 1, LastTrainDate = 0 }
            });
        }

        public async Task AddWord(Word word)
        {
            if (!await IsWordInFlashcards(word))
                await AddRecord(word);
        }

        public async Task<bool> IsWordInFlashcards(Word word)
        {
            return !(await GetFlashcardWordByWord(word) is null);
        }

        public async Task<string> NextTrainDate(Word word)
        {
            FlashcardWord flashcardWord = await GetFlashcardWordByWord(word);
            DateTime lastTrainDate = new DateTime(flashcardWord.LastTrainDate);
            TimeSpan groupDays = new TimeSpan(flashcardWord.Day, 0, 0, 0);
            DateTime nextTrainDate = lastTrainDate.Add(groupDays);

            if(nextTrainDate > DateTime.Now)
                return lastTrainDate.Add(groupDays).ToLongDateString();
            else
                return ContentConstants.MessageWordCanTrain;
        }
    }
}
