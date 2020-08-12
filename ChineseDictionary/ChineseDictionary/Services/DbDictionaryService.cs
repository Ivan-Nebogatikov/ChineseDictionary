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
    public class DbDictionaryService : IDictionaryServiceAsync
    {
        private HttpClient Http;
        private IndexedDBManager DbManager;

        private async Task Load()
        {
            if (!(await DbManager.GetRecords<Word>(DbConstants.StoreName)).Any())
            {
                var response = await Http.GetAsync("https://raw.githubusercontent.com/Oleg42-prog/Lanit-CD/master/short.txt");
                var code = response.StatusCode; // ToDo: condition & error
                await DslParser.DBParseAsync(DbManager, await response.Content.ReadAsStreamAsync());
            }
        }

        public DbDictionaryService(HttpClient Http, IndexedDBManager DbManager)
        {
            this.Http = Http;
            this.DbManager = DbManager;
            Load(); // ToDo: Rewrite
        }

        public async Task<IEnumerable<Word>> SearchAsync(string query, int skip = 0, int take = int.MaxValue)
        {
            if (string.IsNullOrEmpty(query))
                return await SearchByAsync(DbConstants.Chinese, query, skip, take);

            if (query.All(c => c >= 0x61 && c <= 0x7A || c >= 0x41 && c <= 0x5A || char.IsPunctuation(c) || char.IsWhiteSpace(c)))
            {
                // Latin characters
                return await SearchByAsync(DbConstants.PinyinMonotone, query, skip, take);
            }

            if (query.All(c => c < 0x410 || char.IsPunctuation(c) || char.IsWhiteSpace(c)))
            {
                // Latin with diacritic & control characters
                return await SearchByAsync(DbConstants.Pinyin, query, skip, take);
            }

            if (query.All(c => c >= 0x410 && c <= 0x451 || char.IsPunctuation(c) || char.IsWhiteSpace(c)))
            {
                // Cyrillic characters
                return await SearchByAsync(DbConstants.Translations, query, skip, take);
            }

            if (query.All(c => c >= 0x3400 || char.IsPunctuation(c) || char.IsWhiteSpace(c)))
            {
                // Chinese characters
                return await SearchByAsync(DbConstants.Chinese, query, skip, take);
            }

            return null;
        }

        private async Task<IEnumerable<Word>> SearchByAsync(string indexName, string queryValue, int skip, int take)
        {
            var query = new StoreIndexQueryStringContains
            {
                Storename = DbConstants.StoreName,
                IndexName = indexName,
                QueryValue = queryValue
            };

            return (await DbManager.GetAllRecordsByIndexContains<Word>(query)).Skip(skip).Take(take).ToList();
        }

        public async Task<Word> GetByChinese(string chinese)
        {
            var query = new StoreIndexQuery<string>
            {
                Storename = DbConstants.StoreName,
                IndexName = DbConstants.Chinese,
                QueryValue = chinese
            };

            var result = await DbManager.GetRecordByIndex<string, Word>(query);

            return result;
        }
    }
}
