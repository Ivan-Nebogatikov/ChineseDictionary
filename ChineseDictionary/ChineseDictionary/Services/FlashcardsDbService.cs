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

        public async Task<Word[]> GetRandomWordsByGroup(int group, int count)
        {
            Word[] words = new Word[count];
            for (int i = 0; i < count; i++)
                words[i] = await GetRandomWordByGroup(group);

            return words;
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

        public async Task<string[][]> GetRandomTranslationMatrix(Word[] questionWord, int count)
        {
            string[][] translationMatrix = new string[count][];
            for (int i = 0; i < count; i++)
                translationMatrix[i] = await GetRandomTranslations(questionWord[i], 4);

            return translationMatrix;
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
