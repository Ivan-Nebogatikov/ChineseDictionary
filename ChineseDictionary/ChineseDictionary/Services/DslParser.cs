using ChineseDictionary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        private static string RemoveTag(string s)
        {
            Regex regex = new Regex(@"\[[^]]*\]");
            return regex.Replace(s, "");
        }

        public static List<ExtendedWord> Parse(Stream stream)
        {
            List<ExtendedWord> data = new List<ExtendedWord>();
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
                                data.Add(word);

                            word = new ExtendedWord
                            {
                                Chinese = RemoveTag(line)
                            };
                        }
                        else
                            state++;

                        if (state == ParserState.Pinyin)
                            word.Pinyin = RemoveTag(line);

                        if (state == ParserState.Body)
                            word.Translations = RemoveTag(line).Split(";");
                    }
                    else
                        state = ParserState.Space;
                }

                data.Add(word);
            }


            return data;
        }
    }
}
