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

}