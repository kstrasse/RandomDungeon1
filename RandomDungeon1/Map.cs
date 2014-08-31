using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RandomDungeon
{
    public class Map

        //a
    {
        public int visitedCells = 0;
        public Random random = new Random();
        protected Cell[,] cells;
        public Rectangle bounds;
        public DungeonFeatures[,] features;

        public IEnumerable<Point> CellLocations
        {
            get
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        yield return new Point(x, y);
                    }
                }
            }
        }

        public Map(int width, int height)
        {
            cells = new Cell[width, height];
            bounds = new Rectangle(0, 0, width, height);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cells[x, y] = new Cell();
                    cells[x, y].Column = x;
                    cells[x, y].Row = y;

                }
            }
        }


        public Cell this[int x, int y]
        {
            get { return cells[x, y]; }
            set { cells[x, y] = value; }
        }

        public Cell this[Point p]
        {
            get { return cells[p.X, p.Y]; }
            set { cells[p.X, p.Y] = value; }
        }

        public int Width
        {
            get { return cells.GetUpperBound(0) + 1; }
        }

        public int Height
        {
            get { return cells.GetUpperBound(1) + 1; }
        }





        public bool HasAdjacentCellInDirection(Point p, Direction.DirectionType direction)
        {
            switch (direction)
            {
                case Direction.DirectionType.North:
                    return p.Y > 0;

                case Direction.DirectionType.South:
                    return p.Y < Height - 1;

                case Direction.DirectionType.East:
                    return p.X < Width - 1;
                case Direction.DirectionType.West:
                    return p.X > 0;
                default: return false;
            }
        }


        protected Point GetTargetLocation(Point currentLocation, Direction.DirectionType direction)
        {
            Point targetLocation = new Point();
            targetLocation = currentLocation;
            switch (direction)
            {
                case Direction.DirectionType.North:
                    targetLocation.Y--;
                    break;
                case Direction.DirectionType.South:
                    targetLocation.Y++;
                    break;
                case Direction.DirectionType.East:
                    targetLocation.X++;
                    break;
                case Direction.DirectionType.West:
                    targetLocation.X--;
                    break;
            }
            return targetLocation;
        }

        public bool CanMove(Direction.DirectionType direction, Point location)
        {
            switch (direction)
            {
                case Direction.DirectionType.North:
                    if (this[location].NorthSide == Cell.Sidetype.Empty)
                    {
                        return true;
                    }
                    else
                        return false;
                case Direction.DirectionType.South:
                    if (this[location].SouthSide == Cell.Sidetype.Empty)
                    {
                        return true;
                    }
                    else
                        return false;
                case Direction.DirectionType.West:
                    if (this[location].WestSide == Cell.Sidetype.Empty)
                    {
                        return true;
                    }
                    else
                        return false;
                case Direction.DirectionType.East:
                    if (this[location].EastSide == Cell.Sidetype.Empty)
                    {
                        return true;
                    }
                    else
                        return false;
                default: return false;

            }
        }



        public Point CreateSide(Point currentLocation, Direction.DirectionType direction, Cell.Sidetype sidetype)
        {
            Point targetLocation = new Point();
            targetLocation = GetTargetLocation(currentLocation, direction);
            switch (direction)
            {
                case Direction.DirectionType.North:
                    this[currentLocation].NorthSide = sidetype;
                    this[targetLocation].SouthSide = sidetype;
                    break;
                case Direction.DirectionType.South:
                    this[currentLocation].SouthSide = sidetype;
                    this[targetLocation].NorthSide = sidetype;
                    break;
                case Direction.DirectionType.East:
                    this[currentLocation].EastSide = sidetype;
                    this[targetLocation].WestSide = sidetype;
                    break;
                case Direction.DirectionType.West:
                    this[currentLocation].WestSide = sidetype;
                    this[targetLocation].EastSide = sidetype;
                    break;

            }
            return targetLocation;
        }


    }
}
