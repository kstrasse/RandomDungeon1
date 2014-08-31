using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RandomDungeon
{
    public class Tile
    { //
        public string name { get; set; }
        public string ImageCharacter { get; set; }
        public ConsoleColor Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Tile() { }
        public Tile(int x, int y)
            : base()
        {
            this.X = x;
            this.Y = y;

        }

    }
}