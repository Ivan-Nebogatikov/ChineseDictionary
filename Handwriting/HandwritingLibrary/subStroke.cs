using System;
using System.Collections.Generic;
using System.Text;

namespace HandwritingLibrary
{
    public class subStroke
    {
        public readonly double direction;
        public readonly double length;
        public readonly double centerX;
        public readonly double centerY;

        public subStroke(double direction, double length, double centerX, double centerY)
        {
            this.direction = direction;
            this.length = length;
            this.centerX = centerX;
            this.centerY = centerY;
        }
    }
}
