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
        public ExtendedWord GetWordByGroup(int group)
        {
            return null;
        }

        public List<string> GetRandomTranslations(string word, int count)
        {
            return new List<string> { "Привет", "Пока", "Дом", "Холм" };
        }
    }
}
