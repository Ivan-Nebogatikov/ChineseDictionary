using HandwritingLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;


namespace Handwriting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = File.OpenText("orig.json"))
            {
                JObject dictionary = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

                var fileData = File.ReadAllText("中.txt");
                var rawStrokes = JsonConvert.DeserializeObject<List<List<List<double>>>>(fileData);
                var strokes = RawStokeConverter.ParseData(rawStrokes);
                var analyzer = new Analyzer(strokes);
                var subStrokes = analyzer.AnalyzedStrokes;
                var strokesCount = strokes.Count;
                var subStrokesCount = subStrokes.Count;
                int limit = 20;

                Console.WriteLine("Number of the input character's strokes: " + strokesCount);
                Console.WriteLine("Number of the input character's substrokes: " + subStrokesCount);

                Matcher mt = new Matcher(strokesCount, subStrokesCount, subStrokes, limit);
                mt.DoMatching(dictionary);
            }
        }
    }
}
    
