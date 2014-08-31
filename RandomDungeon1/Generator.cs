using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RandomDungeon
{
    public class Generator
    {
        static Random random = new Random();
        RoomGenerator roomGenerator = new RoomGenerator();


        //a


        public void PlaceDoors(Dungeon dungeon)
        {
            foreach (Room room in dungeon.Rooms)
            {
                bool hasNorthDoor = false;
                bool hasSouthDoor = false;
                bool hasWestDoor = false;
                bool hasEastDoor = false;
                foreach (Point cellLocation in room.CellLocations)
                {


                    Point dungeonLocation = new Point(room.bounds.X + cellLocation.X, room.bounds.Y + cellLocation.Y);
                    if ((cellLocation.X == 0) && (dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.West, dungeon)) && (!hasWestDoor))
                    {
                        dungeon.CreateDoor(dungeonLocation, Direction.DirectionType.West);
                        hasWestDoor = true;
                    }
                    if ((cellLocation.X == room.Width - 1) && (dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.East, dungeon)) && (!hasEastDoor))
                    {
                        dungeon.CreateDoor(dungeonLocation, Direction.DirectionType.East);
                        hasEastDoor = true;
                    }
                    if ((cellLocation.Y == 0) && (dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.North, dungeon)) && (!hasNorthDoor))
                    {
                        dungeon.CreateDoor(dungeonLocation, Direction.DirectionType.North);
                        hasNorthDoor = true;
                    }
                    if ((cellLocation.Y == room.Height - 1) && (dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.South, dungeon)) && (!hasSouthDoor))
                    {
                        dungeon.CreateDoor(dungeonLocation, Direction.DirectionType.South);
                        hasSouthDoor = true;
                    }
                }
            }

        }

        public static bool ShouldRemoveDeadend(int deadEndRemovalModifier)
        {
            return random.Next(1, 100) < deadEndRemovalModifier;
        }

        public static void RemoveDeadEnds(Dungeon dungeon, int deadEndRemovalModifier)
        {
            foreach (Point deadEndLocation in dungeon.FindDeadEnds)
            {
                if (ShouldRemoveDeadend(deadEndRemovalModifier))
                {
                    Point currentLocation = deadEndLocation;

                    do
                    {
                        Direction directionPicker = new Direction((Direction.DirectionType)dungeon.CalculateDeadEndCorridorDirection(currentLocation), 100);
                        Direction.DirectionType direction = directionPicker.GetNextDirection();

                        while (!dungeon.HasAdjacentCellInDirection(currentLocation, direction))
                        {
                            if (directionPicker.HasNextDirection)
                                direction = directionPicker.GetNextDirection();
                            else
                                throw new InvalidOperationException("This should not happen");
                        }

                        currentLocation = dungeon.CreateCorridor(currentLocation, direction);
                    } while (dungeon[currentLocation].IsDeadEnd);

                }
            }
        }

        public void SparsifyMaze(Dungeon dungeon, int sparsenessModifier)
        {
            int noOfDeadEndCellsToRemove = (int)Math.Ceiling((decimal)sparsenessModifier / 100 * (dungeon.Width * dungeon.Height));
            IEnumerator<Point> enumerator = dungeon.FindDeadEnds.GetEnumerator();

            for (int i = 0; i < noOfDeadEndCellsToRemove; i++)
            {
                if (!enumerator.MoveNext())
                {
                    enumerator = dungeon.FindDeadEnds.GetEnumerator();
                    if (!enumerator.MoveNext()) break;
                }
                Point point = enumerator.Current;
                dungeon.CreateSide(point, (Direction.DirectionType)dungeon.CalculateDeadEndCorridorDirection(point), Cell.Sidetype.Wall);
            }

        }



        public Dungeon Generate(int width, int height, int changeDirectionModifier, int sparesnessModifier)
        {
            Dungeon dungeon = new Dungeon(width, height);
            Point location = dungeon.PickRandomCellMarkVisited();
            Direction.DirectionType previousDirection = Direction.DirectionType.North;

            while (dungeon.visitedCells < dungeon.Height * dungeon.Width)
            {
                Direction DirectionPicker = new Direction(previousDirection, changeDirectionModifier);
                Direction.DirectionType direction = DirectionPicker.GetNextDirection();
                while (!dungeon.HasAdjacentCellInDirection(location, direction) || dungeon.AdjacentCellInDirectionVisited(location, direction))
                {
                    if (DirectionPicker.HasNextDirection)
                    {
                        direction = DirectionPicker.GetNextDirection();
                    }

                    else
                    {
                        location = dungeon.PickRandomVisitedCell(location);
                        DirectionPicker = new Direction(previousDirection, changeDirectionModifier);
                        direction = DirectionPicker.GetNextDirection();
                    }

                }

                location = dungeon.CreateCorridor(location, direction);
                dungeon.MarkCellsAsVisited(location);
                previousDirection = direction;
            }

            SparsifyMaze(dungeon, sparesnessModifier);
            RemoveDeadEnds(dungeon, 70);
            roomGenerator.GenerateRooms(dungeon);
            PlaceDoors(dungeon);
            return dungeon;
        }
    }
}
