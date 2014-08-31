using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.ObjectModel;

namespace RandomDungeon
{
    public class Room : Map
    {
        public ReadOnlyCollection<Room> Rooms
        {
            get { return rooms.AsReadOnly(); }
        }

        public readonly List<Room> rooms = new List<Room>();

        public Room(int Width, int Height)
            : base(Width, Height)
        {
            cells = new Cell[Width, Height];
            bounds = new Rectangle(0, 0, Width, Height);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cell cell = new Cell();
                    cell.WestSide = (x == 0) ? Cell.Sidetype.Wall : Cell.Sidetype.Empty;
                    cell.EastSide = (x == Width - 1) ? Cell.Sidetype.Wall : Cell.Sidetype.Empty;
                    cell.NorthSide = (y == 0) ? Cell.Sidetype.Wall : Cell.Sidetype.Empty;
                    cell.SouthSide = (y == Height - 1) ? Cell.Sidetype.Wall : Cell.Sidetype.Empty;
                    this[x, y] = cell;
                }
            }
        }

        public void SetLocation(Point location)
        {
            bounds.X = location.X;
            bounds.Y = location.Y;

        }





    }
}
