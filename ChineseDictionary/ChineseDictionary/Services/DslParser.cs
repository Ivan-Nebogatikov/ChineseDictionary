using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TG.Blazor.IndexedDB;
using ChineseDictionary.Constants;
using System.Text;

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

        // Seems horrible, but it's okay
        private static Dictionary<char, char> monotoneTable = new Dictionary<char, char>
        {
            ['ā'] = 'a', ['á'] = 'a', ['ǎ'] = 'a', ['à'] = 'a',
            ['ē'] = 'e', ['é'] = 'e', ['ě'] = 'e', ['è'] = 'e',
            ['ī'] = 'i', ['í'] = 'i', ['ǐ'] = 'i', ['ì'] = 'i',
            ['ō'] = 'o', ['ó'] = 'o', ['ǒ'] = 'o', ['ò'] = 'o',
            ['ū'] = 'u', ['ǖ'] = 'u', ['ú'] = 'u', ['ǘ'] = 'u', ['ǔ'] = 'u', ['ǚ'] = 'u', ['ù'] = 'u', ['ǜ'] = 'u', ['ü'] = 'u',
            ['Ā'] = 'A', ['Á'] = 'A', ['Ǎ'] = 'A', ['À'] = 'A',
            ['Ē'] = 'E', ['É'] = 'E', ['Ě'] = 'E', ['È'] = 'E',
            ['Ī'] = 'I', ['Í'] = 'I', ['Ǐ'] = 'I', ['Ì'] = 'I',
            ['Ō'] = 'O', ['Ó'] = 'O', ['Ǒ'] = 'O', ['Ò'] = 'O',
            ['Ū'] = 'U', ['Ǖ'] = 'U', ['Ú'] = 'U', ['Ǘ'] = 'U', ['Ǔ'] = 'U', ['Ǚ'] = 'U', ['Ù'] = 'U', ['Ǜ'] = 'U', ['Ü'] = 'U'
        };

        private static string Monotone(string s)
        {
            StringBuilder ss = new StringBuilder();
            foreach (char c in s)
                if (monotoneTable.ContainsKey(c))
                    ss.Append(monotoneTable[c]);
                else
                    ss.Append(c);
            return ss.ToString();
        }

        private static async Task AddRecord(IndexedDBManager DbManager, Word word)
        {
            await DbManager.AddRecord(new StoreRecord<Word>
            {
                Storename = DbConstants.StoreName,
                Data = word
            });
        }

        public static async Task DBParseAsync(IndexedDBManager DbManager, Stream stream)
        {
            using StreamReader sr = new StreamReader(stream, System.Text.Encoding.Unicode);
            string line;
            ParserState state = ParserState.Space;
            Word word = null;

            while ((line = sr.ReadLine()) != null)
            {
                if (line != "" && line[0] != '#')
                {
                    if (line[0] != ' ')
                    {
                        state = ParserState.Title;

                        if (word != null)
                            await AddRecord(DbManager, word);

                        word = new Word
                        {
                            Chinese = RemoveTags(line)
                        };
                    }
                    else
                        state++;

                    if (state == ParserState.Pinyin)
                    {
                        word.Pinyin = RemoveTags(line);
                        word.PinyinMonotone = Monotone(word.Pinyin);
                    }

                    if (state == ParserState.Body)
                    {
                        word.RelativeWords = GetRelativeWords(line);
                        word.Translations = RemoveTags(line).Split(";");
                    }
                }
                else
                    state = ParserState.Space;
            }

            await AddRecord(DbManager, word);
        }
    }
}
