using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomDungeon
{
    public class Direction
    {
        Random random = new Random();
        public List<DirectionType> directionsPicked = new List<DirectionType>();
        public DirectionType previousDirection;
        public int changeDirectionModifier;

        public Direction(DirectionType previousDirection, int changeDirectionModifier)
        {
            this.previousDirection = previousDirection;
            this.changeDirectionModifier = changeDirectionModifier;
        }

        public bool MustChangeDirection
        {
            get
            {
                return ((directionsPicked.Count > 0) || (changeDirectionModifier > random.Next(100)));
            }
        }
        public bool HasNextDirection
        {
            get { return directionsPicked.Count < 4; }

        }

        public DirectionType GetNextDirection()
        {
            if (!HasNextDirection)
                throw new InvalidOperationException("No directions Available");

            DirectionType direction;

            do
            {
                direction = MustChangeDirection ? PickDifferentDirection() : previousDirection;
            }
            while (directionsPicked.Contains(direction));
            directionsPicked.Add(direction);

            return direction;


        }




        private DirectionType PickDifferentDirection()
        {
            DirectionType direction;
            do
            {
                direction = (DirectionType)random.Next(4);
            }
            while ((direction == previousDirection) && (directionsPicked.Count < 3));

            return direction;
        }
        public enum DirectionType
        {
            North,
            South,
            East,
            West
        }
    }
}
