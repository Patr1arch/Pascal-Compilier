using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace myPascal
{
    public class Program
    {
        
        // TODO: Return pair<statusCode, file(or filename)> ?
        public static (int, string) ProceedArguments(List<string> args)
        {
            switch (args[0])
            {
                case "-l":
                    // TODO: Start Lexer session
                    if (File.Exists(Environment.CurrentDirectory + "/" + args[1]))
                    {
                        return (0, args[1]);
                    }
                    else
                    {
                        Console.WriteLine("Error! File not found.");
                        return (-1, "");
                    }
                default:
                    if (File.Exists(Environment.CurrentDirectory + "/" + args[0]))
                    {
                        return (1, args[0]);
                    }
                    else
                    {
                        Console.WriteLine("Error! File not found.");
                        return (-1, "");
                    }
            }
        }
        
        static void Main(string[] args)
        {
            var obj = ProceedArguments(args.ToList());
            Lexer lexer = new Lexer(obj.Item2);
            switch (obj.Item1)
            {
                case 0:
                    
                    break;
                case 1:
                    break;
            }
        }
    }
}