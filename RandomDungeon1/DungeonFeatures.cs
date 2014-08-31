using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RandomDungeon
{
    public class DungeonFeatures : Tile
    {
        //a
        
        public enum TileType
        {
            Rock,
            Floor,
            Door
        
        }
        

        public TileType KindOfTile { get; set; }

        public DungeonFeatures(Point p)
        {
            X = p.X;
            Y = p.Y;
        }


    }
}