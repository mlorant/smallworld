using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Point
    {
        private int x;
        private int y;

        public int X { get { return this.x; } }
        public int Y { get { return this.y; } }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public abstract class Case : ICase
    {
        private const int SIZE = 50;
    }
}
