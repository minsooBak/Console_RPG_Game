﻿
using System.Diagnostics;

namespace RPG_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map map = new();
            if (map.CreatePlayer())
                map.DrawMap();
        }
    }
}
