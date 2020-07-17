﻿using ChineseDictionary.Models;
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

        private async void Load()
        {
            if (!(await DbManager.GetRecords<Word>("Dictionary")).Any())
            {
                var response = await Http.GetAsync("https://raw.githubusercontent.com/Oleg42-prog/Lanit-CD/master/short.txt");
                var code = response.StatusCode; // ToDo: condition & error
                DslParser.DBParseAsync(DbManager, await response.Content.ReadAsStreamAsync());
            }
        }

        public DbDictionaryService(HttpClient Http, IndexedDBManager DbManager, DbStore DbStore)
        {
            this.Http = Http;
            this.DbManager = DbManager;
            Load();
        }

        private async Task<IEnumerable<ExtendedWord>> SearchByAsync(string indexName, string queryValue, int skip = 0, int take = int.MaxValue)
        {
            var query = new StoreIndexQueryStringContains
            {
                Storename = DbConstants.StoreName,
                IndexName = indexName,
                QueryValue = queryValue
            };

            // Question
            return (await DbManager.GetAllRecordsByIndexContains<ExtendedWord>(query)).Skip(skip).Take(take).ToList();
        }

        public async Task<IEnumerable<ExtendedWord>> SearchByChineseAsync(string chinese, int skip = 0, int take = int.MaxValue)
        {
            return await SearchByAsync(DbConstants.Chinese, chinese, skip, take);
        }

        public async Task<IEnumerable<ExtendedWord>> SearchByTranslationAsync(string translation, int skip = 0, int take = int.MaxValue)
        {
            return await SearchByAsync(DbConstants.Translations, translation, skip, take);
        }
    }
}