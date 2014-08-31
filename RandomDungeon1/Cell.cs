using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RandomDungeon
{
    public class Cell
    {
        //a
        public static int CellSize = 15;
        public enum Sidetype
        {
            Wall,
            Empty,
            Door
        }

        public int Wallcount
        {
            get
            {
                int wallCount = 0;
                if (NorthSide == Sidetype.Wall)
                    wallCount++;
                if (SouthSide == Sidetype.Wall)
                    wallCount++;
                if (EastSide == Sidetype.Wall)
                    wallCount++;
                if (WestSide == Sidetype.Wall)
                    wallCount++;
                return wallCount;
            }
        }

        public bool IsDeadEnd
        {
            get { return Wallcount == 3; }
        }

        public bool IsRock
        {
            get { return Wallcount == 4; }
        }

        public bool IsCorridor
        {
            get { return Wallcount <= 3; }
        }


        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsVisited { get; set; }
        public Sidetype NorthSide { get; set; }
        public Sidetype SouthSide { get; set; }
        public Sidetype EastSide { get; set; }
        public Sidetype WestSide { get; set; }

        public void Draw(Graphics g)
        {
            if (NorthSide == Sidetype.Wall)
                g.DrawLine(Pens.Black, Column * CellSize, Row * CellSize, (Column + 1) * CellSize, Row * CellSize);
            if (SouthSide == Sidetype.Wall)
                g.DrawLine(Pens.Black, Column * CellSize, (Row + 1) * CellSize, (Column + 1) * CellSize, (Row + 1) * CellSize);
            if (EastSide == Sidetype.Wall)
                g.DrawLine(Pens.Black, (Column + 1) * CellSize, Row * CellSize, (Column + 1) * CellSize, (Row + 1) * CellSize);
            if (WestSide == Sidetype.Wall)
                g.DrawLine(Pens.Black, Column * CellSize, Row * CellSize, Column * CellSize, (Row + 1) * CellSize);
            if (NorthSide == Sidetype.Door)
            {
                g.DrawLine(Pens.Black, Column * CellSize, Row * CellSize, (Column + 1) * CellSize, Row * CellSize);
                g.DrawRectangle(Pens.Black, Column * CellSize + CellSize / 4, Row * CellSize - CellSize / 4, CellSize / 2, CellSize / 2);
            }
            if (SouthSide == Sidetype.Door)
            {
                g.DrawLine(Pens.Black, Column * CellSize, (Row + 1) * CellSize, (Column + 1) * CellSize, (Row + 1) * CellSize);
                g.DrawRectangle(Pens.Black, Column * CellSize + CellSize / 4, (Row + 1) * CellSize - CellSize / 4, CellSize / 2, CellSize / 2);
            }
            if (EastSide == Sidetype.Door)
            {
                g.DrawLine(Pens.Black, (Column + 1) * CellSize, Row * CellSize, (Column + 1) * CellSize, (Row + 1) * CellSize);
                g.DrawRectangle(Pens.Black, (Column + 1) * CellSize - CellSize / 4, Row * CellSize + CellSize / 4, CellSize / 2, CellSize / 2);
            }
            if (WestSide == Sidetype.Door)
            {
                g.DrawLine(Pens.Black, Column * CellSize, Row * CellSize, Column * CellSize, (Row + 1) * CellSize);
                g.DrawRectangle(Pens.Black, Column * CellSize - CellSize / 4, Row * CellSize + CellSize / 4, CellSize / 2, CellSize / 2);
            }
        }


    }
}
