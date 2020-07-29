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

        public async Task<FlashcardWord> GetRandomWordByGroup(int group)
        {
            var query = new StoreIndexQuery<int>
            {
                Storename = DbConstants.FlashcardsStoreName,
                IndexName = DbConstants.Day,
                QueryValue = group
            };

            var result = await DbManager.GetAllRecordsByIndex<int, FlashcardWord>(query);
            return result[rand.Next(result.Count)];
        }

        public async Task<List<string>> GetRandomTranslations(FlashcardWord questionWord, int count)
        {
            ExtendedWord word;
            List<ExtendedWord> words = await DbManager.GetRecords<ExtendedWord>(DbConstants.StoreName);
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

            var query = new StoreIndexQuery<string>
            {
                Storename = DbConstants.StoreName,
                IndexName = DbConstants.Chinese,
                QueryValue = questionWord.Chinese
            };

            string[] rightTranslations = (await DbManager.GetRecordByIndex<string, ExtendedWord>(query)).Translations.ToArray<string>();
            string rightTranslation = rightTranslations[rand.Next(rightTranslations.Length)];
            randomTranslations.Insert(rand.Next(randomTranslations.Count), rightTranslation);

            return randomTranslations;
        }

        public async Task<bool> IsCorrectTranslation(string chinese, string translate)
        {
            var query = new StoreIndexQuery<string>
            {
                Storename = DbConstants.StoreName,
                IndexName = DbConstants.Chinese,
                QueryValue = chinese
            };

            var result = await DbManager.GetRecordByIndex<string, ExtendedWord>(query);
            return result.Translations.Contains(translate);
        }

        public async Task MoveFlashcards(List<FlashcardWord> words, int group)
        {
            StoreRecord<FlashcardWord> record;
            foreach (FlashcardWord word in words)
            {
                word.Day = group;
                record = new StoreRecord<FlashcardWord>
                {
                    Storename = DbConstants.FlashcardsStoreName,
                    Data = word
                };
                
                await DbManager.UpdateRecord(record);
            }
        }
    }
}
