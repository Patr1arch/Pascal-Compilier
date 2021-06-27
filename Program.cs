using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace myPascal
{
    public class Program
    {
        public static int ProceedArguments(List<string> args)
        {
            switch (args[0])
            {
                case "-l":
                    // TODO: Start Lexer session
                    if (File.Exists(Environment.CurrentDirectory + "/" + args[1]))
                    {
                        Console.WriteLine("Hello World!");
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("Error! File not found.");
                        return 1;
                    }
                default:
                    if (File.Exists(Environment.CurrentDirectory + "/" + args[0]))
                    {
                        Console.WriteLine("Hello World!");
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("Error! File not found.");
                        return 1;
                    }
            }
        }
        
        static void Main(string[] args)
        {
            ProceedArguments(args.ToList());
        }
    }
}