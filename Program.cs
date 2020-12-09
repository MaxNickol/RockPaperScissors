using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace RockPaperScissors
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int len = args.Length;
            bool exists = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (Array.IndexOf(args, args[i]) != Array.LastIndexOf(args, args[i]))
                {
                    exists = true;
                }
            }

            if (len < 3 || len % 2 == 0 || exists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You should input only odd numbers of unique symbols to play. Start with 3.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine("The example is: \nRockPaperScissors.exe rock paper scissors lizard Spock (Total: 5)");
                Console.WriteLine();
            }
            else
            {
                new Game(args).StartGame();
            }
        }
    }
}