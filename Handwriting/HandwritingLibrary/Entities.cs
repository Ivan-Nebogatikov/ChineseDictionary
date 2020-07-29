using System;
using System.Collections.Generic;
using System.Text;

namespace HandwritingLibrary
{
    public struct Point
    {
        public double X;
        public double Y;
    }

    public class SubStroke
    {
        public double Dir;
        public double Len;
        public double CenterX;
        public double CenterY;
    }

    public class Stroke // создание одной черты
    {
        public List<Point> Points = new List<Point>();
    }
}
