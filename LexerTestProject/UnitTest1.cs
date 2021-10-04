//using Microsoft.VisualStudio.TestPlatform.TestHost;

using System;
using System.Collections.Generic;
using NUnit.Framework;
using myPascal;

namespace LexerTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StartLexer()
        {
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"-l", "TestPascalFiles\\test.pas"}), (0, "TestPascalFiles\\test.pas"));
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"TestPascalFiles\\test.pas"}), (1, "TestPascalFiles\\test.pas"));
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"-l", "TestPascalFiles\\testtrash.pas"}), (-1, ""));
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"TestPascalFiles\\testtrash.pas"}), (-1, ""));
            // TODO: Handle this problem if you'll have free time
            //Assert.AreEqual(Program.ProceedArguments(new List<string>(){"-aba", "testtrash.pas"}), 1);
        }

        [Test]
        public void SymIdent()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\firstSymIdent.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:1\tType: " +
                                                      "Identifier\tSource Code: a\tValue: \"a\"");
        }

        [Test]
        public void Keyword()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\Keyword.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:1\tType: " +
                                                      "Keyword\tSource Code: and\tValue: \"and\"");
        }
        
        [Test]
        public void KeywordAndIdent()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\KeywordAndIdent.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:1\tType: " +
                                                      "Keyword\tSource Code: program\tValue: \"program\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:9\tType: " +
                                                      "Identifier\tSource Code: Hello\tValue: \"Hello\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:9\tType: " +
                                                      "Identifier\tSource Code: Hello\tValue: \"Hello\"");
        }

        [Test]
        public void WhiteSpaceChecker()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\Whitespaces.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 3:1\tType: " +
                                                      "Keyword\tSource Code: program\tValue: \"program\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 3:12\tType: " +
                                                      "Identifier\tSource Code: Hello\tValue: \"Hello\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:2\tType: " +
                                                      "Identifier\tSource Code: World\tValue: \"World\"");
        }

        [Test]
        public void IntLiteral()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\IntLiteral.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:1\tType: " +
                                                      "IntegerLiteral\tSource Code: 123\tValue: \"123\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 2:1\tType: " +
                                                      "IntegerLiteral\tSource Code: 456\tValue: \"456\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 3:1\tType: " +
                                                      "IntegerLiteral\tSource Code: 7\tValue: \"7\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 4:1\tType: " +
                                                      "IntegerLiteral\tSource Code: 8\tValue: \"8\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 5:1\tType: " +
                                                      "IntegerLiteral\tSource Code: 90\tValue: \"90\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:1\tType: " +
                                                      "IntegerLiteral\tSource Code: 0001\tValue: \"1\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 7:1\tType: " +
                                                      "IntegerLiteral\tSource Code: 0000\tValue: \"0\"");
        }
        
        [Test]
        public void HexesAndBinaries()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\HexesAndBinaries.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:1\tType: " +
                                                      "IntegerLiteral\tSource Code: $DEADBEEF\tValue: \"3735928559\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 2:1\tType: " +
                                                      "IntegerLiteral\tSource Code: $DEAD\tValue: \"57005\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 3:1\tType: " +
                                                      "IntegerLiteral\tSource Code: $beef\tValue: \"48879\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 4:1\tType: " +
                                                      "IntegerLiteral\tSource Code: %00000\tValue: \"0\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 5:1\tType: " +
                                                      "IntegerLiteral\tSource Code: %10101\tValue: \"21\"");
        }
        
}