using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using Waher.Security;
using Waher.Security.SHA3;

namespace RockPaperScissors
{
    internal class Game
    {
        private Status status = Status.InProgress;

        private int AIMove;

        private int PlayerMove;

        private string Key;

        private string HMAC;

        private string[] Args;

        public Game(string[] args)
        {
            this.Args = args;
        }

        public void StartGame()
        {
            while (status != Status.End)
            {
                this.Key = KeyGenerator();
                this.AIMove = ComputerMove();
                this.HMAC = HMACGenerator();
                Console.WriteLine();
                Console.WriteLine($"HMAC:{this.HMAC}");
                this.PlayerMove = Move();

                if (this.PlayerMove == 0)
                {
                    status = Status.End;
                }
                else
                {
                    Console.WriteLine($"Your move: {this.Args[this.PlayerMove - 1]}");
                    Console.WriteLine($"Computer move: {this.Args[this.AIMove - 1]}");
                    Status gameResult = CalculateWinner(this.AIMove, this.PlayerMove);

                    switch (gameResult)
                    {
                        case Status.Lost:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You lost!");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case Status.Win:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("You win!");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case Status.Draw:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("It's Draw!");
                            break;

                        default:
                            Console.WriteLine("Unknown winner");
                            break;
                    }

                    Console.WriteLine($"HMAC Key: {this.Key}");
                }
            }
        }

        public int Move()
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < this.Args.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {this.Args[i]}");
            }

            Console.WriteLine("0 - Exit");

            Console.WriteLine("Enter your move:");
            bool result = int.TryParse(Console.ReadLine(), out var parsedMove);

            while (result != true)
            {
                Console.WriteLine("Enter your move:");
                result = int.TryParse(Console.ReadLine(), out parsedMove);
            }

            return parsedMove;
        }

        public int ComputerMove()
        {
            Random rndInt = new Random();

            int move = rndInt.Next(1, this.Args.Length + 1);

            return move;
        }

        public Status CalculateWinner(int a, int b)
        {
            Status status;

            int half = (this.Args.Length - 1) / 2;

            int len = this.Args.Length;

            int PlDif = len - this.PlayerMove;
            int CompDif = len - this.AIMove;

            if (PlDif > CompDif)
            {
                int res = PlDif - CompDif;

                status = res <= half ? Status.Lost : Status.Win;
            }
            else if (PlDif == CompDif)
            {
                status = Status.Draw;
            }
            else
            {
                int res = CompDif - PlDif;
                status = res <= half ? Status.Win : Status.Lost;
            }

            return status;
        }

        public string KeyGenerator()
        {
            //Another way to build a key;
            //RandomNumberGenerator rndBytes = RandomNumberGenerator.Create();
            //byte[] bytes = new byte[24];
            //string generatedStr = String.Empty;
            //RandomNumberGenerator.Fill(bytes);
            //generatedStr = Convert.ToBase64String(bytes).ToUpper();

            StringBuilder generatedStr = new StringBuilder();

            byte[] bytesArr = new byte[16];

            RandomNumberGenerator.Fill(bytesArr);

            for (int i = 0; i < bytesArr.Length; i++)
            {
                generatedStr.Append(bytesArr[i].ToString("x2"));
            }

            return generatedStr.ToString().ToUpper();
        }

        public string HMACGenerator()
        {
            string keyedStr = String.Concat(this.Key, this.Args[this.AIMove - 1]);

            var hmacString = new StringBuilder();

            SHA3_256 sha = new SHA3_256();

            byte[] bytedValue = Encoding.UTF8.GetBytes(keyedStr);

            byte[] shaBytes = sha.ComputeVariable(bytedValue);

            foreach (var val in shaBytes)
            {
                hmacString.Append(val.ToString("x2"));
            }

            return hmacString.ToString().ToUpper();
        }
    }
}