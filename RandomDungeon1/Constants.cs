using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomDungeon
{
    public static class Constants
    {

        //a
        public readonly static string FloorImage = ".";
        public readonly static string RockImage = "#";
        public readonly static string DoorImage = "+";
        public readonly static string PlayerImage = "@";

        public readonly static ConsoleColor DoorColor = ConsoleColor.DarkCyan;
        public readonly static ConsoleColor FloorColor = ConsoleColor.White;
        public readonly static ConsoleColor RockColor = ConsoleColor.White;
        public readonly static ConsoleColor PlayerColor = ConsoleColor.White;
    }
}
