using HandwritingLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Handwriting
{
    class Program
    {
        

        static void Main(string[] args)
        {
            var fileData = File.ReadAllText("也.txt");
            var rawStrokes = JsonConvert.DeserializeObject<List<List<List<double>>>>(fileData);
            var strokes = RawStokeConverter.ParseData(rawStrokes);
            var analyzer = new Analyzer(strokes);
            var subStrokes = analyzer.AnalyzedStrokes;
            var strokesCount = strokes.Count;
            var subStrokesCount = subStrokes.Count;
            Console.WriteLine(strokesCount);
            Console.WriteLine(subStrokesCount);

            Matcher mt = new Matcher(strokesCount, subStrokesCount);
            mt.DoMatching();


        }
    }
}
