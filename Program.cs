using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Program
    {
        static List<string> OutPut = new List<string>();
        static int count = 0;
        static string[,] Grid =
        {
            { "7", "8", "9", "/" },
            { "4", "5", "6", "*" },
            { "1", "2", "3", "-" },
            { "0", "(", ")", "+" },
            { ".", "D" ,"C", "=" }
        };
        static int X = 0;
        static int Y = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                Draw();
                KeyReader();
                Console.Clear();
            }    
        }
        static void Draw()
        {
            int bruv = 26 - OutPut.Count;
            Console.WriteLine(new string('=', 28));
            Console.WriteLine($"|{string.Join("",OutPut)}{new string(' ',bruv)}|");
            Console.WriteLine(new string('=', 28));
            Console.WriteLine($"|{new string(' ', 26)}|");
            KeyDrawer();
            Console.WriteLine($"|{new string(' ',26)}|");
            Console.WriteLine(new string('=', 28));
        }
        static void KeyDrawer()
        {
            for (int i = 0; i < 5;i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Grid[i, j] == Grid[Y, X])
                    {
                        Console.Write($"| >{Grid[i,j]}< |");
                    }else Console.Write($"|  {Grid[i, j]}  |");
                }
                Console.WriteLine("");
            }
        }
        static void KeyReader()
        {
            var KeyPressed = Console.ReadKey();
            if ( KeyPressed.Key == ConsoleKey.RightArrow)
            {
                if(X != 3) X++;
            }
            else if (KeyPressed.Key == ConsoleKey.LeftArrow)
            {
                if(X != 0) X--;
            }
            if(KeyPressed.Key == ConsoleKey.DownArrow)
            {
                if (Y != 4) Y++;
            }
            else if(KeyPressed.Key == ConsoleKey.UpArrow)
            {
                if ( Y != 0) Y--;
            }
            if (KeyPressed.Key == ConsoleKey.Enter)
            {
                if (Grid[Y,X] == "C")
                {
                    OutPut.Clear();
                }
                if (Grid[Y,X] == "D")
                {
                    if (count != 0)
                    {
                        OutPut.RemoveAt(OutPut.Count - 1);
                        count--;
                    }
                }
                if (Grid[Y, X] != "=" && Grid[Y,X] != "C" && Grid[Y,X] != "D")
                {
                    if (count != 26)
                    {
                        OutPut.Add(Grid[Y, X]);
                        count++;
                    }
                }
                else
                {
                    Calculate();
                }
            }
        }
        static void Calculate()
        {
        }
    }
}
