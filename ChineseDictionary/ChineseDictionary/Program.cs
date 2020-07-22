using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ChineseDictionary.Services;
using TG.Blazor.IndexedDB;
using ChineseDictionary.Constants;

namespace ChineseDictionary
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = DbConstants.DbName;
                dbStore.Version = 2;

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = DbConstants.StoreName,
                    PrimaryKey = new IndexSpec { Name = DbConstants.Id, KeyPath = DbConstants.Id, Auto = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec { Name = DbConstants.Chinese, KeyPath = DbConstants.Chinese, Auto = false },
                        new IndexSpec { Name = DbConstants.Pinyin, KeyPath = DbConstants.Pinyin, Auto = false },
                        new IndexSpec { Name = DbConstants.Translations, KeyPath = DbConstants.Translations, Auto = false },
                        new IndexSpec { Name = DbConstants.RelativeWords, KeyPath = DbConstants.RelativeWords, Auto = false },
                    }
                });

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Flashcards",
                    PrimaryKey = new IndexSpec { Name = DbConstants.Id, KeyPath = DbConstants.Id, Auto = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec { Name = DbConstants.Chinese, KeyPath = DbConstants.Chinese, Auto = false },
                        new IndexSpec { Name = DbConstants.Day, KeyPath = DbConstants.Day, Auto = false },
                    }
                });
            });

            builder.Services.AddTransient<IDictionaryServiceAsync, DbDictionaryService>();
            builder.Services.AddTransient<IFlashcardsService, FlashcardsService>();
            await builder.Build().RunAsync();
        }
    }
}
