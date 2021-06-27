using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using myPascal.Tokenizer;

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
                        Console.WriteLine("Hello World!");
                        return (0, args[1]);
                    }
                    else
                    {
                        Console.WriteLine("Error! File not found.");
                        return (1, "");
                    }
                default:
                    if (File.Exists(Environment.CurrentDirectory + "/" + args[0]))
                    {
                        Console.WriteLine("Hello World!");
                        return (0, args[0]);
                    }
                    else
                    {
                        Console.WriteLine("Error! File not found.");
                        return (1, "");
                    }
            }
        }
        
        static void Main(string[] args)
        {
            var obj = ProceedArguments(args.ToList());
            if (obj.Item1 == 0)
            {
                Lexer lexer = new Lexer(obj:q.Item2);
                // while (lexer.NextToken() == 'a')
                // {
                //     
                // }
            }
        }
    }
}