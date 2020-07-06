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

namespace ChineseDictionary
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IDictionaryService>(new MockDictionaryService());
            builder.Services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = "TheFactory";
                dbStore.Version = 2;

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Dictionary",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec { Name = "chinese", KeyPath = "chinese", Auto = false },
                        new IndexSpec{Name="pinyin", KeyPath = "pinyin", Auto=false},
                        new IndexSpec{Name="translations", KeyPath = "translations", Auto=false}

                    }
                });
            });
            await builder.Build().RunAsync();
        }
    }
}
