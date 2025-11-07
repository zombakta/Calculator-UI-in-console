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
            int bruv = 26 - string.Join("", OutPut).Length;
            Console.WriteLine(new string('=', 28));
            Console.WriteLine($"|{string.Join("", OutPut)}{new string(' ', bruv)}|");
            Console.WriteLine(new string('=', 28));
            Console.WriteLine($"|{new string(' ', 26)}|");
            KeyDrawer();
            Console.WriteLine($"|{new string(' ', 26)}|");
            Console.WriteLine(new string('=', 28));
        }
        static void KeyDrawer()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Grid[i, j] == Grid[Y, X])
                    {
                        Console.Write($"| >{Grid[i, j]}< |");
                    }
                    else Console.Write($"|  {Grid[i, j]}  |");
                }
                Console.WriteLine("");
            }
        }
        static void KeyReader()
        {
            var KeyPressed = Console.ReadKey();
            if (KeyPressed.Key == ConsoleKey.RightArrow)
            {
                if (X != 3) X++;
            }
            else if (KeyPressed.Key == ConsoleKey.LeftArrow)
            {
                if (X != 0) X--;
            }
            if (KeyPressed.Key == ConsoleKey.DownArrow)
            {
                if (Y != 4) Y++;
            }
            else if (KeyPressed.Key == ConsoleKey.UpArrow)
            {
                if (Y != 0) Y--;
            }
            if (KeyPressed.Key == ConsoleKey.Enter)
            {
                if (Grid[Y, X] == "C")
                {
                    OutPut.Clear();
                }
                if (Grid[Y, X] == "D")
                {
                    if (count != 0)
                    {
                        OutPut.RemoveAt(OutPut.Count - 1);
                        count--;
                    }
                }
                if (Grid[Y, X] != "=" && Grid[Y, X] != "C" && Grid[Y, X] != "D")
                {
                    if (count != 26)
                    {
                        OutPut.Add(Grid[Y, X]);
                        count++;
                    }
                }
                else if (Grid[Y, X] == "=")
                {
                    Calculate();
                }
            }
        }
        static bool Delim(char c) => char.IsWhiteSpace(c);
        static bool IsOp(char c) => c == '+' || c == '-' || c == '*' || c == '/';
        static bool IsUnary(char c) => c == '+' || c == '-';
        static int Priority(int op)
        {
            if (op < 0) 
                return 3;

            char c = (char)op;
            if (c == '+' || c == '-') return 1;
            if (c == '*' || c == '/') return 2;
            return -1;
        }
        static void ProcessOp(Stack<double> st, int op)
        {
            if (op < 0)
            {
                double l = st.Pop();
                switch ((char)(-op))
                {
                    case '+': st.Push(l); break;
                    case '-': st.Push(-l); break;
                }
            }
            else
            {
                double r = st.Pop();
                double l = st.Pop();
                switch ((char)op)
                {
                    case '+': st.Push(l + r); break;
                    case '-': st.Push(l - r); break;
                    case '*': st.Push(l * r); break;
                    case '/': st.Push(l / r); break;
                }
            }
        }
        public static double Evaluate(string s)
        {
            var st = new Stack<double>();
            var op = new Stack<int>();
            bool mayBeUnary = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (Delim(s[i]))
                    continue;

                if (s[i] == '(')
                {
                    op.Push('(');
                    mayBeUnary = true;
                }
                else if (s[i] == ')')
                {
                    while (op.Peek() != '(')
                        ProcessOp(st, op.Pop());

                    op.Pop();
                    mayBeUnary = false;
                }
                else if (IsOp(s[i]))
                {
                    int curOp = s[i];
                    if (mayBeUnary && IsUnary((char)curOp))
                        curOp = -curOp;

                    while (op.Count > 0 && (
                        (curOp >= 0 && Priority(op.Peek()) >= Priority(curOp)) ||
                        (curOp < 0 && Priority(op.Peek()) > Priority(curOp))
                        ))
                    {
                        ProcessOp(st, op.Pop());
                    }

                    op.Push(curOp);
                    mayBeUnary = true;
                }
                else
                {
                    if (!char.IsDigit(s[i]) && s[i] != '.')
                    {
                        throw new ArgumentException($"Invalid character in expression: '{s[i]}'");
                    }
                    string numStr = "";
                    while (i < s.Length && (char.IsDigit(s[i]) || s[i] == '.'))
                    {
                        numStr += s[i];
                        i++;
                    }
                    if (!double.TryParse(numStr, out double number))
                        throw new ArgumentException($"Invalid number in expression: '{numStr}'");
                    i--;
                    st.Push(number);
                    mayBeUnary = false;
                }
            }
            while (op.Count > 0)
            {
                ProcessOp(st, op.Pop());
            }
            return st.Peek();
        }
        static void Calculate()
        {
            if (OutPut.Count == 0)
                return;
            double result = Evaluate(string.Join("", OutPut));
            OutPut.Clear();
            OutPut.Add(result.ToString());
        }
    }
}
