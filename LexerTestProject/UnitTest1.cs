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
        public void Test2()
        {
            Lexer testLexer = new Lexer("firstSym.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:1\tType: " +
                                                      "Identifier\tSource Code: a\tValue: \"a\"");
        }
    }
}