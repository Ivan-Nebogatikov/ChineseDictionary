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
    public class FlashcardsService : IFlashcardsService
    {
        private IndexedDBManager DbManager;
        private Random rand;

        public FlashcardsService(IndexedDBManager DbManager)
        {
            this.DbManager = DbManager;
            this.rand = new Random();
        }

        /*public async Task<ExtendedWord> GetRandomWordByGroup(int group)
        {
            var query = new StoreIndexQuery<int>
            {
                Storename = DbConstants.FlashcardsStoreName,
                IndexName = DbConstants.Day,
                QueryValue = group
            };

            var result = await DbManager.GetAllRecordsByIndex<int, ExtendedWord>(query);
            return result[rand.Next(result.Count)];
        }*/

        public async Task<ExtendedWord> GetRandomWordByGroup(int group)
        {
            var query = new StoreIndexQuery<int>
            {
                Storename = DbConstants.StoreName,
                IndexName = DbConstants.Id,
                QueryValue = group
            };

            return await DbManager.GetRecordByIndex<int, ExtendedWord>(query);
        }

        public List<string> GetRandomTranslations(string word, int count)
        {
            return new List<string> { word, "Привет", "Пока", "Дом" };
        }
    }
}
