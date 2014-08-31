using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.ObjectModel;

namespace RandomDungeon
{
    public class Dungeon : Map
    {
        
        Point playerPoint = new Point(0,0);
        Player player;
        public ReadOnlyCollection<Room> Rooms
        {
            get { return rooms.AsReadOnly(); }
        }
        
        
        
        public DungeonFeatures[,] tiles;
        public readonly List<Room> rooms = new List<Room>();

        public List<Point> AllCells = new List<Point>();
        public Dungeon(int width, int height) : base(width, height) { }



        public IEnumerable<Point> Rock
        {
            get
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (this[x, y].IsRock)
                            yield return new Point(x, y);
                    }
                }
            }
        }

       
        

        public List<Point> EnumerateCellLocations()
        {
            foreach (Point point in CellLocations)
                AllCells.Add(point);
            return AllCells;
        }

        public void Draw(Graphics g)
        {
            foreach (Point cell in CellLocations)
                this[cell].Draw(g);

            foreach (Point point in Rock)
            {
                g.FillRectangle(Brushes.Black, point.X * Cell.CellSize, point.Y * Cell.CellSize, Cell.CellSize, Cell.CellSize);
            }
        }

        public bool CheckRock(Point p)
        {
            if (this[p].IsRock)
                return true;
            else
                return false;
        }



        public IEnumerable<Point> FindDeadEnds
        {
            get
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (this[x, y].IsDeadEnd)
                            yield return new Point(x, y);
                    }
                }
            }
        }

        public bool AdjacentCellInDirectionIsCorridor(Point location, Direction.DirectionType direction, Dungeon dungeon)
        {
            if (HasAdjacentCellInDirection(location, direction))
            {
                Point target = GetTargetLocation(location, direction);

                switch (direction)
                {
                    case Direction.DirectionType.North:
                        return dungeon[target].IsCorridor;
                    case Direction.DirectionType.West:
                        return dungeon[target].IsCorridor;
                    case Direction.DirectionType.South:
                        return dungeon[target].IsCorridor;
                    case Direction.DirectionType.East:
                        return dungeon[target].IsCorridor;
                    default: throw new InvalidOperationException();
                }
            }
            else return false;
        }

        public IEnumerable<Point> CorridorCellLocations
        {
            get
            {
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                        if (this[x, y].IsCorridor) yield return new Point(x, y);
            }
        }

        public Direction.DirectionType CalculateDeadEndCorridorDirection(Point currentLocation)
        {
            if (this[currentLocation].NorthSide == Cell.Sidetype.Empty)
                return Direction.DirectionType.North;
            if (this[currentLocation].SouthSide == Cell.Sidetype.Empty)
                return Direction.DirectionType.South;
            if (this[currentLocation].EastSide == Cell.Sidetype.Empty)
                return Direction.DirectionType.East;
            if (this[currentLocation].WestSide == Cell.Sidetype.Empty)
                return Direction.DirectionType.West;

            throw new InvalidOperationException();
        }


        public Point PickRandomCellMarkVisited()
        {
            Point currentlocation = new Point(random.Next(Width), random.Next(Height));
            this[currentlocation].IsVisited = true;
            visitedCells++;
            return currentlocation;

        }

        public bool AdjacentCellInDirectionVisited(Point p, Direction.DirectionType direction)
        {
            switch (direction)
            {
                case Direction.DirectionType.North:
                    return this[p.X, p.Y - 1].IsVisited;
                case Direction.DirectionType.South:
                    return this[p.X, p.Y + 1].IsVisited;
                case Direction.DirectionType.East:
                    return this[p.X + 1, p.Y].IsVisited;
                case Direction.DirectionType.West:
                    return this[p.X - 1, p.Y].IsVisited;

                default: throw new InvalidOperationException("Out of bounds");
            }
        }




        public Point CreateCorridor(Point currentLocation, Direction.DirectionType direction)
        {
            currentLocation = CreateSide(currentLocation, direction, Cell.Sidetype.Empty);
            return currentLocation;
        }

        public Point PickRandomVisitedCell(Point location)
        {

            Point newLocation = new Point(random.Next(Width), random.Next(Height));
            while (this[newLocation].IsVisited == false)
            {
                newLocation = new Point(random.Next(Width), random.Next(Height));
            }

            return newLocation;
        }

        public void MarkCellsAsVisited(Point location)
        {
            this[location].IsVisited = true;
            visitedCells++;
        }




        public Point CreateDoor(Point location, Direction.DirectionType direction)
        {
            return CreateSide(location, direction, Cell.Sidetype.Door);
        }

        public DungeonFeatures[,] ExpandToTiles(Dungeon dungeon)
        {
            tiles = new DungeonFeatures[dungeon.Width * 2 + 1, dungeon.Height * 2 + 1];
            for (int x = 0; x < dungeon.Width * 2 + 1; x++)
            {
                for (int y = 0; y < dungeon.Height * 2 + 1; y++)
                {
                    Point tilePoint = new Point(x, y);
                    tiles[x, y] = new DungeonFeatures(tilePoint);
                    tiles[x, y].KindOfTile = DungeonFeatures.TileType.Rock;
                }
            }

            foreach (Room room in dungeon.Rooms)
            {
                Point minPoint = new Point(room.bounds.Location.X * 2 + 1, room.bounds.Location.Y * 2 + 1);
                Point maxPoint = new Point(room.bounds.Right * 2, room.bounds.Bottom * 2);
                for (int i = minPoint.X; i < maxPoint.X; i++)
                {
                    for (int j = minPoint.Y; j < maxPoint.Y; j++)
                    {
                        tiles[i, j].KindOfTile = DungeonFeatures.TileType.Floor;

                    }
                }

            }

            foreach (Point cellLocation in dungeon.CorridorCellLocations)
            {
                Point tileLocation = new Point(cellLocation.X * 2 + 1, cellLocation.Y * 2 + 1);
                tiles[tileLocation.X, tileLocation.Y].KindOfTile = DungeonFeatures.TileType.Floor;
                if (dungeon[cellLocation].NorthSide == Cell.Sidetype.Empty)
                    tiles[tileLocation.X, tileLocation.Y - 1].KindOfTile = DungeonFeatures.TileType.Floor;
                if (dungeon[cellLocation].NorthSide == Cell.Sidetype.Door)
                    tiles[tileLocation.X, tileLocation.Y - 1].KindOfTile = DungeonFeatures.TileType.Door;
                if (dungeon[cellLocation].SouthSide == Cell.Sidetype.Empty)
                    tiles[tileLocation.X, tileLocation.Y + 1].KindOfTile = DungeonFeatures.TileType.Floor;
                if (dungeon[cellLocation].SouthSide == Cell.Sidetype.Door)
                    tiles[tileLocation.X, tileLocation.Y + 1].KindOfTile = DungeonFeatures.TileType.Door;
                if (dungeon[cellLocation].WestSide == Cell.Sidetype.Empty)
                    tiles[tileLocation.X - 1, tileLocation.Y].KindOfTile = DungeonFeatures.TileType.Floor;
                if (dungeon[cellLocation].WestSide == Cell.Sidetype.Door)
                    tiles[tileLocation.X - 1, tileLocation.Y].KindOfTile = DungeonFeatures.TileType.Door;
                if (dungeon[cellLocation].EastSide == Cell.Sidetype.Empty)
                    tiles[tileLocation.X + 1, tileLocation.Y].KindOfTile = DungeonFeatures.TileType.Floor;
                if (dungeon[cellLocation].EastSide == Cell.Sidetype.Door)
                    tiles[tileLocation.X + 1, tileLocation.Y].KindOfTile = DungeonFeatures.TileType.Door;
            }
            
           
            GetImageCharacters();
            //tiles[player.X, player.Y].ImageCharacter = Constants.PlayerImage;
            return tiles;
        }

        public void GetImageCharacters()
        {
            foreach (DungeonFeatures tile in tiles)
            {
                if (tile.KindOfTile == DungeonFeatures.TileType.Floor)
                {
                    tile.ImageCharacter = Constants.FloorImage;
                    tile.Color = Constants.FloorColor;
                }
                if (tile.KindOfTile == DungeonFeatures.TileType.Door)
                {
                    tile.ImageCharacter = Constants.DoorImage;
                    tile.Color = Constants.DoorColor;
                }
                if (tile.KindOfTile == DungeonFeatures.TileType.Rock)
                {
                    tile.ImageCharacter = Constants.RockImage;
                    tile.Color = Constants.RockColor;
                }
            }


        }

        public void DrawToConsole()
        {
            Console.Clear();
            tiles = ExpandToTiles(this);
            for (int y = 0; y < Width * 2 + 1; y++)
            {
                for (int x = 0; x < Height * 2 + 1; x++)
                {
                    Console.BackgroundColor = tiles[x, y].Color;
                    Console.Write(tiles[x, y].ImageCharacter);

                }

                Console.Write("\r\n");
            }

        }

        



    }



}
