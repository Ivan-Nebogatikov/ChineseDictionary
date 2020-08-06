﻿using HandwritingLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Handwriting
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileData = File.ReadAllText("中.txt");
            var rawStrokes = JsonConvert.DeserializeObject<List<List<List<double>>>>(fileData);
            var strokes = RawStokeConverter.ParseData(rawStrokes);
            var analyzer = new Analyzer(strokes);
            var subStrokes = analyzer.AnalyzedStrokes;
            var strokesCount = strokes.Count;
            var subStrokesCount = subStrokes.Count;
            int limit = 20;
           
            Console.WriteLine("The amount of the input character's strokes" + strokesCount);
            Console.WriteLine("The amount of the input character's substrokes" + subStrokesCount);

            //MatchCollector mc = new MatchCollector(limit);
            Matcher mt = new Matcher(strokesCount, subStrokesCount, subStrokes, limit);

            mt.DoMatching();
            mt.getMatches();
            
            //mc.AddMatch(new CharacterMatch('a', 0.42));
            //mc.AddMatch(new CharacterMatch('b', 0.314));
            //mc.AddMatch(new CharacterMatch('c', 0.4));
            //mc.AddMatch(new CharacterMatch('d', 0.21));
            //mc.AddMatch(new CharacterMatch('e', 0.9));
            //char[] matches = Matcher.matches;
            //foreach (char match in matches)
            //{
            //    Console.WriteLine(match);
            //}
        }
    }
}
    
