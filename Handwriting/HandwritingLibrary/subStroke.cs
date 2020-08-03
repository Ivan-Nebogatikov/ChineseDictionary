using System;
using System.Collections.Generic;
using System.Text;

namespace HandwritingLibrary
{
    public class subStroke
    {
        public subStroke(double direction, double length, double centerX, double centerY)
        {
            Direction = direction;
            Length = length;
            CenterX = centerX;
            CenterY = centerY;


        }

        public readonly double Direction;
        public readonly double Length;
        public readonly double CenterX;
        public readonly double CenterY;

    }
}
