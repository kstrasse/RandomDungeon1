using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RandomDungeon
{
    public class RoomGenerator
    {
        Random random = new Random();


        public void PlaceRoom(Point location, Room room, Dungeon dungeon)
        {
            room.SetLocation(location);

            for (int x = 0; x < room.Width; x++)
            {
                for (int y = 0; y < room.Height; y++)
                {
                    Point dungeonLocation = new Point(location.X + x, location.Y + y);
                    dungeon[dungeonLocation].NorthSide = room[x, y].NorthSide;
                    dungeon[dungeonLocation].SouthSide = room[x, y].SouthSide;
                    dungeon[dungeonLocation].EastSide = room[x, y].EastSide;
                    dungeon[dungeonLocation].WestSide = room[x, y].WestSide;

                    if ((x == 0) && (dungeon.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.West))) dungeon.CreateSide(dungeonLocation, Direction.DirectionType.West, Cell.Sidetype.Wall);
                    if ((x == room.Width - 1) && (dungeon.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.East))) dungeon.CreateSide(dungeonLocation, Direction.DirectionType.East, Cell.Sidetype.Wall);
                    if ((y == 0) && (dungeon.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.North))) dungeon.CreateSide(dungeonLocation, Direction.DirectionType.North, Cell.Sidetype.Wall);
                    if ((y == room.Height - 1) && (dungeon.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.South))) dungeon.CreateSide(dungeonLocation, Direction.DirectionType.South, Cell.Sidetype.Wall);
                }
            }


            dungeon.rooms.Add(room);
        }

        public int CalculateRoomPlacementScore(Point location, Room room, Dungeon dungeon)
        {
            if (dungeon.bounds.Contains(new Rectangle(location, new Size(room.Width + 1, room.Height + 1))))
            {
                int roomPlacementScore = 0;

                for (int x = 0; x < room.Width; x++)
                {
                    for (int y = 0; y < room.Height; y++)
                    {
                        Point dungeonLocation = new Point(location.X + x, location.Y + y);

                        if ((room.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.North) && dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.North, dungeon)))
                            roomPlacementScore++;
                        if ((room.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.South) && dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.South, dungeon)))
                            roomPlacementScore++;
                        if ((room.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.East) && dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.East, dungeon)))
                            roomPlacementScore++;
                        if ((room.HasAdjacentCellInDirection(dungeonLocation, Direction.DirectionType.West) && dungeon.AdjacentCellInDirectionIsCorridor(dungeonLocation, Direction.DirectionType.West, dungeon)))
                            roomPlacementScore++;

                        if (dungeon[dungeonLocation].IsCorridor)
                            roomPlacementScore += 3;

                        foreach (Room dungeonRoom in dungeon.Rooms)
                            if (dungeonRoom.bounds.Contains(dungeonLocation))
                                roomPlacementScore += 100;
                    }
                }

                return roomPlacementScore;

            }
            else
            {
                return int.MaxValue;
            }
        }

        public Room CreateRoom(int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight)
        {
            Room room = new Room(random.Next(minRoomWidth, maxRoomWidth), random.Next(minRoomHeight, maxRoomHeight));
            return room;
        }


        public void PlaceDoors(Dungeon dungeon)
        {
            foreach (Room room in dungeon.Rooms)
            {
            }
        }
        public Point CreateDoor(Point location, Direction.DirectionType direction, Room room)
        {
            return room.CreateSide(location, direction, Cell.Sidetype.Door);
        }


        public void PlaceRooms(int noOfRoomsToPlace, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight, Dungeon dungeon)
        {
            for (int roomCounter = 0; roomCounter < noOfRoomsToPlace; roomCounter++)
            {
                Room room = CreateRoom(minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight);
                int bestRoomPlacementScore = int.MaxValue;
                Point? bestRoomPlacementLocation = null;

                foreach (Point currentRoomPlacementLocation in dungeon.CorridorCellLocations)
                {

                    int currentRoomPlacementScore = CalculateRoomPlacementScore(currentRoomPlacementLocation, room, dungeon);

                    if (currentRoomPlacementScore < bestRoomPlacementScore)
                    {
                        bestRoomPlacementScore = currentRoomPlacementScore;
                        bestRoomPlacementLocation = currentRoomPlacementLocation;
                    }

                }

                if (bestRoomPlacementLocation != null)
                    PlaceRoom(bestRoomPlacementLocation.Value, room, dungeon);
            }

        }

        public void GenerateRooms(Dungeon dungeon)
        {
            PlaceRooms(7, 3, 8, 3, 8, dungeon);

        }
    }
}
