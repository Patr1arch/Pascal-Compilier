//using Microsoft.VisualStudio.TestPlatform.TestHost;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using NUnit.Framework;
using myPascal;

namespace LexerTestProject
{
    public class Tests
    {
        private Lexer _lexer;
        private Parser _parser;
        private static string startPath = "TestPascalFiles";
        private static string lexerTestType = "Lexer";
        private static string parserTestType = "Parser";
        private static readonly List<string> TestTypes = new List<string>() {lexerTestType, parserTestType}; // TODO: Make it in config file?
        
        [SetUp]
        public void Setup()
        {
            _lexer = new Lexer();
            _parser = new Parser();
        }

        [Test]
        public void StartLexer()
        {
            Assert.AreEqual(Program.ProceedArguments(new List<string>() {"-l", "TestPascalFiles\\test.pas"}),
                (0, "TestPascalFiles\\test.pas"));
            Assert.AreEqual(Program.ProceedArguments(new List<string>() {"TestPascalFiles\\test.pas"}),
                (2, "TestPascalFiles\\test.pas"));
            Assert.AreEqual(Program.ProceedArguments(new List<string>() {"-l", "TestPascalFiles\\testtrash.pas"}),
                (-1, ""));
            Assert.AreEqual(Program.ProceedArguments(new List<string>() {"TestPascalFiles\\testtrash.pas"}), (-1, ""));
            // TODO: Handle this problem if you'll have free time
            //Assert.AreEqual(Program.ProceedArguments(new List<string>(){"-aba", "testtrash.pas"}), 1);
        }

        [Test]
        public void LexerTests()
        {
            foreach (var testType in TestTypes)
            {
                Console.WriteLine($"Start tests in {testType} compilier part");
                var lexerPath = startPath + "\\" + testType;
                var filePaths = Directory.GetFiles(lexerPath + "\\In");
                foreach (var filePath in filePaths)
                {
                    _lexer.SetFilePath(filePath);
                    var pattern = filePath.Split("\\").Last().Split(".")[0];
                    var outFilePath = lexerPath + "\\Out\\" + pattern + "Out.txt";
                    var outFile = new StreamReader(outFilePath, Encoding.UTF8);
                    Console.WriteLine($"Start working with \n {filePath} \n source file and \n {outFilePath} \n output file\n");
                    if (testType == lexerTestType)
                    {
                        var line = outFile.ReadLine();
                        while (line != null)
                        {
                            try 
                            {
                                Assert.AreEqual(_lexer.GetNextLexem().ToString(), line, $"In test file {filePath} \n in output {outFilePath}");
                            }
                            catch (Exception e)
                            {
                                Assert.AreEqual(e.Message, line, $"In test file {filePath} \n in output {outFilePath} \n" +
                                                                 $"{e.Message} \n {line} \n");
                                break;
                            }
                            line = outFile.ReadLine();
                        }
                    }
                    else
                    {
                        _parser.SetParser(_lexer);
                        try
                        {
                            var exp = _parser.ParseProgram().Print();
                            var act = outFile.ReadToEnd().Replace("\r\n", "\n");
                            Assert.AreEqual(exp, act, 
                                $"In test file {filePath} \n in output {outFilePath} \n with epxected file \n" +
                                $"{@exp} \n\n And actual file \n {@act}");
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual(@e.Message, @outFile.ReadLine(), $"In test file {filePath} \n in output {outFilePath}");
                        }
                    }
                }
            }
        }
        
        [Test]
        public void GetAllLexems()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\GetAllLexems.pas");
            var res = testLexer.GetAllLexems();
            // TODO: Add more source code to verify this test method
            Assert.AreEqual(res[0].ToString(), "Coordinates: 1:1\tType: " +
                                               "StringLiteral\tSource Code: \'string\'\tValue: \"\'string\'\"");
            Assert.AreEqual(res[1].ToString(), "Coordinates: 1:9\tType: " +
                                               "Separator\tSource Code: ;\tValue: \";\"");
        }
    }
}