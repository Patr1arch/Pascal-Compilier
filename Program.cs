using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using myPascal.Lexems;

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
                    if (File.Exists(Environment.CurrentDirectory + "\\" + args[1]))
                    {
                        return (0, args[1]);
                    }
                    else
                    {
                        Console.WriteLine($"{args[1]} Fatal: File not found.");
                        return (-1, "");
                    }
                default:
                    if (File.Exists(Environment.CurrentDirectory + "/" + args[0]))
                    {
                        return (1, args[0]);
                    }
                    else
                    {
                        Console.WriteLine($"{args[0]} Fatal: File not found.");
                        return (-1, "");
                    }
            }
        }
        
        static void Main(string[] args)
        {
            var obj = ProceedArguments(args.ToList());
            switch (obj.Item1)
            {
                case 0:
                case 1:
                    Lexer lexer = new Lexer(obj.Item2);
                    try
                    {
                        var res = lexer.GetAllLexems();
                        foreach (var el in res)
                        {
                            Console.WriteLine(el.ToString());
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                    break;
            }
        }
    }
}