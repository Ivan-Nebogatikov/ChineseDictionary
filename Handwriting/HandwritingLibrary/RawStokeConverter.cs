using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandwritingLibrary
{
    public class RawStokeConverter
    {
        public static List<Stroke> ParseData(List<List<List<double>>> rawData)
        {
            var res = new List<Stroke>();
            foreach(var stroke in rawData)
            {
                var resStroke = new Stroke();
                resStroke.Points = stroke.Select(x => new Point { X = x[0], Y = x[1] }).ToList();
                if (resStroke.Points.Any())
                {
                    res.Add(resStroke);
                }
            }
            return res;
        }
    }
}
