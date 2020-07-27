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

        private static void DbMessage(object sender, IndexedDBNotificationArgs args)
        {
            Console.WriteLine("Message: " + args.Message);
        }

        public async Task<FlashcardWord> GetRandomWordByGroup(int group)
        {
            Console.WriteLine("group: " + group.ToString());
            var query = new StoreIndexQuery<int>
            {
                Storename = DbConstants.FlashcardsStoreName,
                IndexName = DbConstants.Day,
                QueryValue = group
            };

            //var result = await DbManager.GetAllRecordsByIndex<int, FlashcardWord>(query);

            Console.WriteLine("Stores: ");
            foreach (var s in DbManager.Stores)
            {
                Console.WriteLine(s.Name);
                Console.WriteLine(DbConstants.FlashcardsStoreName);
                Console.WriteLine(s.Name == DbConstants.FlashcardsStoreName);
                Console.WriteLine("_____");
            }

            DbManager.ActionCompleted += DbMessage;

            
            var result = await DbManager.GetRecords<FlashcardWord>(DbConstants.FlashcardsStoreName);
            Console.WriteLine("result is: ");
            if (result is null)
            {
                Console.WriteLine("Null");
            }
            else
            {
                Console.WriteLine("Ok");
                Console.WriteLine(result.Count);
            }

            return result[rand.Next(result.Count)];
        }

        public List<string> GetRandomTranslations(string word, int count)
        {
            return new List<string> { word, "Привет", "Пока", "Дом" };
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
    }
}
