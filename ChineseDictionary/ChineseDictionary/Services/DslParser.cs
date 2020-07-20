using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TG.Blazor.IndexedDB;
using ChineseDictionary.Constants;

namespace ChineseDictionary.Services
{
    public static class DslParser
    {
        enum ParserState
        {
            Space,
            Title,
            Pinyin,
            Body
        }

        private static Regex removeRegex = new Regex(@"\[[^]]*\]", RegexOptions.Compiled);
        private static Regex relativeWordRegex = new Regex(@"\[ref\]([^[]*)\[\/ref\]", RegexOptions.Compiled);

        private static string RemoveTags(string s)
        {
            return removeRegex.Replace(s, "");
        }

        private static List<string> GetRelativeWords(string s)
        {
            List<string> relativeWords = new List<string>();
            MatchCollection matches = relativeWordRegex.Matches(s);

            if (matches.Any())
                foreach (Match match in matches)
                    relativeWords.Add(match.Groups[1].Value);

            return relativeWords;
        }

        public static async void DBParseAsync(IndexedDBManager DbManager, Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.Unicode))
            {
                string line;
                ParserState state = ParserState.Space;
                ExtendedWord word = null;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "" && line[0] != '#')
                    {
                        if (line[0] != ' ')
                        {
                            state = ParserState.Title;

                            if (word != null)
                                await DbManager.AddRecord<ExtendedWord>(new StoreRecord<ExtendedWord>
                                {
                                    Storename = DbConstants.StoreName,
                                    Data = word
                                });

                            word = new ExtendedWord
                            {
                                Chinese = RemoveTags(line)
                            };
                        }
                        else
                            state++;

                        if (state == ParserState.Pinyin)
                            word.Pinyin = RemoveTags(line);

                        if (state == ParserState.Body)
                        {
                            word.RelativeWords = GetRelativeWords(line);
                            word.Translations = RemoveTags(line).Split(";");
                        }
                    }
                    else
                        state = ParserState.Space;
                }

                await DbManager.AddRecord<ExtendedWord>(new StoreRecord<ExtendedWord> { 
                    Storename = DbConstants.StoreName, 
                    Data = word 
                });
            }
        }
    }
}
