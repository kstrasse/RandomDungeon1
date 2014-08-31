using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomDungeon
{
    public class Compiler
    { 

        static void Main(string[] args)
        {
            Generator generator = new Generator();
            Dungeon dungeon = generator.Generate(25, 25, 75, 70);
            dungeon.DrawToConsole();
            Console.ReadLine();

        }


    }
}