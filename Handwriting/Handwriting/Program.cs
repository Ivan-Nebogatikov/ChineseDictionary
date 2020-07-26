using HandwritingLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Handwriting
{
    class Program
    {
        public static int strokesCount { get; internal set; }
        public static int subStrokesCount { get; internal set; }

        static void Main(string[] args)
        {
            var fileData = File.ReadAllText("D:/Projects/КитайскийСловарь/ChineseDictionary/Handwriting/Handwriting/中.txt");
            var rawStrokes = JsonConvert.DeserializeObject<List<List<List<double>>>>(fileData);
            var strokes = RawStokeConverter.ParseData(rawStrokes);
            var analyzer = new Analyzer(strokes);
            var subStrokes = analyzer.AnalyzedStrokes;
            strokesCount = strokes.Count;
            subStrokesCount = subStrokes.Count;
            Console.WriteLine(strokesCount);
            Console.WriteLine(subStrokesCount);

            Matcher mt = new Matcher();
            Matcher.DoMatching();

        }
    }
}
