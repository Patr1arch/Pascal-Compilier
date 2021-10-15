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
        private static string startPath = "TestPascalFiles\\WIP";
        
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
            var lexerPath = startPath + "\\Lexer";
            var filePaths = Directory.GetFiles(lexerPath + "\\In");
            foreach (var filePath in filePaths)
            {
                _lexer.SetFilePath(filePath);
                var pattern = filePath.Split("\\").Last().Split(".")[0];
                var outFilePath = lexerPath + "\\Out\\" + pattern + "Out.txt";
                var outFile = new StreamReader(outFilePath, Encoding.UTF8);
                var line = outFile.ReadLine();
                Console.WriteLine($"Start working with \n {filePath} \n source file and \n {outFilePath} \n output file\n");
                while (line != null)
                {
                    try 
                    {
                        Assert.AreEqual(_lexer.GetNextLexem().ToString(), line, $"In test file {filePath} \n in output {outFilePath}");
                    }
                    catch (Exception e)
                    {
                        Assert.AreEqual(e.Message, line, $"In test file {filePath} \n in output {outFilePath}");
                        break;
                    }
                    line = outFile.ReadLine();
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
        :
        public class ParserTest
        {
            [Test]
            public void SimpleEquation()
            {
                Lexer lex = new Lexer("TestPascalFiles\\FirstLexems.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseExpr().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb, 
                    new StreamReader("TestPascalFiles\\ParserOutput\\SimpleEquationOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void Parents()
            {
                Lexer lex = new Lexer("TestPascalFiles\\Parents.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseExpr().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb, 
                    new StreamReader("TestPascalFiles\\ParserOutput\\ParentsOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void Substraction()
            {
                Lexer lex = new Lexer("TestPascalFiles\\ClassicSub.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseExpr().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb, 
                    new StreamReader("TestPascalFiles\\ParserOutput\\SubstractionOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void PriorityParents()
            {
                Lexer lex = new Lexer("TestPascalFiles\\PriorityParents.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseExpr().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb, 
                    new StreamReader("TestPascalFiles\\ParserOutput\\PriorityParentsOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void HardParents()
            {
                Lexer lex = new Lexer("TestPascalFiles\\HardParents.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseExpr().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb, 
                    new StreamReader("TestPascalFiles\\ParserOutput\\HardParentsOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }

            [Test]
            public void ArithmeticExample()
            {
                Lexer lex = new Lexer("TestPascalFiles\\ArithmeticExample.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseExpr().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\ArithmeticExampleOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void ProblemWithRParent()
            {
                Lexer lex = new Lexer("TestPascalFiles\\ProblemWithRParent.pas");
                Parser parser = new Parser(lex);
                Exception e = Assert.Throws<Exception>(() => parser.ParseExpr());
                Assert.AreEqual(e.Message, "TestPascalFiles\\ProblemWithRParent.pas(1, 8) " +
                                           $"Fatal: Unexpected lexem 2");
            }
            
            [Test]
            public void AssignStmt()
            {
                Lexer lex = new Lexer("TestPascalFiles\\AssignStmt.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\AssignStmtOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void CallIdent()
            {
                Lexer lex = new Lexer("TestPascalFiles\\CallIdent.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\CallIdentOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void WhileStmt()
            {
                Lexer lex = new Lexer("TestPascalFiles\\ParseWhile.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\ParseWhileOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void RepeatStmt()
            {
                Lexer lex = new Lexer("TestPascalFiles\\RepeatStmt.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\RepeatStmtOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void ForStmt()
            {
                Lexer lex = new Lexer("TestPascalFiles\\ForStmt.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\ForStmtOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void IfStmt()
            {
                Lexer lex = new Lexer("TestPascalFiles\\IfStmt.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\IfStmtOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void CaseStmt()
            {
                Lexer lex = new Lexer("TestPascalFiles\\CaseStmt.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\CaseStmtOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void NilAndUnaries()
            {
                Lexer lex = new Lexer("TestPascalFiles\\NilAndUnaries.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\NilAndUnariesOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void ArrayAccess()
            {
                Lexer lex = new Lexer("TestPascalFiles\\ArrayAccess.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\ArrayAccessOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void FieldAccess()
            {
                Lexer lex = new Lexer("TestPascalFiles\\FieldAccess.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\FieldAccessOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void Pointers()
            {
                Lexer lex = new Lexer("TestPascalFiles\\Pointers.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\PointersOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }
            
            [Test]
            public void EmptyStmts()
            {
                Lexer lex = new Lexer("TestPascalFiles\\EmptyStmts.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb, "");
            }
            
            [Test]
            public void FloatsPar()
            {
                Lexer lex = new Lexer("TestPascalFiles\\FloatsPar.pas");
                Parser parser = new Parser(lex);
                var deb = parser.ParseProgram().Print();
                Debug.WriteLine(deb);
                Assert.AreEqual(deb,
                    new StreamReader("TestPascalFiles\\ParserOutput\\FloatsParOut.txt")
                        .ReadToEnd().Replace("\r\n", "\n")); // For Windows
            }

        }
    }
}