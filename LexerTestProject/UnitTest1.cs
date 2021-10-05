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
        
        [Test]
        public void CommendedCode()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\CommentedCode.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 12:1\tType: " +
                                                      "Keyword\tSource Code: program\tValue: \"program\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 12:9\tType: " +
                                                      "Identifier\tSource Code: Hello\tValue: \"Hello\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 14:1\tType: " +
                                                      "Identifier\tSource Code: prog\tValue: \"prog\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 15:1\tType: " +
                                                      "Keyword\tSource Code: program\tValue: \"program\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 15:9\tType: " +
                                                      "Identifier\tSource Code: Hello\tValue: \"Hello\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 17:1\tType: " +
                                                      "Keyword\tSource Code: program\tValue: \"program\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 18:7\tType: " +
                                                      "Identifier\tSource Code: Hello\tValue: \"Hello\"");
        }
        
        [Test]
        public void Separators()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\Separators.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:1\tType: " +
                                                      "Identifier\tSource Code: a\tValue: \"a\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:2\tType: " +
                                                      "Separator\tSource Code: [\tValue: \"[\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:3\tType: " +
                                                      "IntegerLiteral\tSource Code: 5\tValue: \"5\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:4\tType: " +
                                                      "Separator\tSource Code: ]\tValue: \"]\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:5\tType: " +
                                                      "Separator\tSource Code: ;\tValue: \";\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 2:1\tType: " +
                                                      "Separator\tSource Code: (\tValue: \"(\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 2:2\tType: " +
                                                      "IntegerLiteral\tSource Code: 42\tValue: \"42\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 2:4\tType: " +
                                                      "Separator\tSource Code: )\tValue: \")\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 2:5\tType: " +
                                                      "Separator\tSource Code: ;\tValue: \";\"");
        }
        [Test]
        public void Operators()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\Operators.pas");
            testLexer.NextLexem();
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 1:3\tType: " +
                                                      "Operator\tSource Code: -\tValue: \"-\"");
            testLexer.SkipLexems(3);
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 2:3\tType: " +
                                                      "Operator\tSource Code: +\tValue: \"+\"");
            testLexer.SkipLexems(3);
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 3:3\tType: " +
                                                      "Operator\tSource Code: <<\tValue: \"<<\"");
            
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 3:6\tType: " +
                                                      "Identifier\tSource Code: b\tValue: \"b\"");
            
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 3:7\tType: " +
                                                      "Separator\tSource Code: ;\tValue: \";\"");
            
        }
        
        [Test]
        public void Example()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\Example.pas");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 5:1\tType: " +
                                                      "Keyword\tSource Code: program\tValue: \"program\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 5:9\tType: " +
                                                      "Identifier\tSource Code: Hello\tValue: \"Hello\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 5:14\tType: " +
                                                      "Separator\tSource Code: ;\tValue: \";\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:1\tType: " +
                                                      "Keyword\tSource Code: var\tValue: \"var\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:5\tType: " +
                                                      "Identifier\tSource Code: a\tValue: \"a\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:6\tType: " +
                                                      "Separator\tSource Code: ,\tValue: \",\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:7\tType: " +
                                                      "Identifier\tSource Code: b\tValue: \"b\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:8\tType: " +
                                                      "Separator\tSource Code: ,\tValue: \",\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:9\tType: " +
                                                      "Identifier\tSource Code: c\tValue: \"c\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:10\tType: " +
                                                      "Separator\tSource Code: :\tValue: \":\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:11\tType: " +
                                                      "Keyword\tSource Code: integer\tValue: \"integer\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 6:18\tType: " +
                                                      "Separator\tSource Code: ;\tValue: \";\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 8:5\tType: " +
                                                      "Identifier\tSource Code: f\tValue: \"f\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 8:7\tType: " +
                                                      "Separator\tSource Code: :\tValue: \":\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 8:9\tType: " +
                                                      "Keyword\tSource Code: real\tValue: \"real\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 8:13\tType: " +
                                                      "Separator\tSource Code: ;\tValue: \";\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 9:1\tType: " +
                                                      "Keyword\tSource Code: begin\tValue: \"begin\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 10:3\tType: " +
                                                      "Identifier\tSource Code: writeln\tValue: \"writeln\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 10:10\tType: " +
                                                      "Separator\tSource Code: (\tValue: \"(\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 10:11\tType: " +
                                                      "Identifier\tSource Code: a\tValue: \"a\"");
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 10:12\tType: " +
                                                      "Separator\tSource Code: )\tValue: \")\"");
            testLexer.NextLexem();
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 11:1\tType: " +
                                                      "Keyword\tSource Code: end.\tValue: \"end.\"");
            
        }
        
        [Test]
        public void ExsUnclosedComment()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\ExsUnclosedComment.pas");
            testLexer.SkipLexems(4);
            Exception e = Assert.Throws<Exception>(() => testLexer.NextLexem());
            Assert.AreEqual(e.Message, "TestPascalFiles\\ExsUnclosedComment.pas(2, 5) Fatal: Detected unclosed comment");
        }
        
        [Test]
        public void ExcLongString()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\ExcLongString.pas");
            testLexer.SkipLexems(10);
            testLexer.NextLexem();
            Exception e = Assert.Throws<Exception>(() => testLexer.NextLexem());
            Assert.AreEqual(e.Message, "TestPascalFiles\\ExcLongString.pas(4, 5) Fatal: Length of string " +
                                       "more than 255");
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
        
        [Test]
        public void InvalidCharInComm()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\InvalidCharInComm.pas");
            Exception e = Assert.Throws<Exception>(() => testLexer.NextLexem());
            Assert.AreEqual(e.Message,
                "TestPascalFiles\\InvalidCharInComm.pas(2, 5) Fatal: Detected invalid character");
        }
        
        [Test]
        public void StringLiterals()
        {
            Lexer testLexer = new Lexer("TestPascalFiles\\StringLiterals.pas");
            testLexer.SkipLexems(17);
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 7:11\tType: " +
                "StringLiteral\tSource Code: \'a\'\tValue: \"\'a\'\"");
            testLexer.SkipLexems(3);
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 8:10\tType: " + 
                                                      "StringLiteral\tSource Code: \'" +
                                                      "A tabulator character: ??? is easy to embed and its not integer" +
                                                      "\'\tValue: \"\'" +
                                                      "A tabulator character: ??? is easy to embed and its not integer" +
                                                      "\'\"");
            testLexer.SkipLexems(3);
            testLexer.NextLexem();
            Assert.AreEqual(testLexer.GetLexemName(), "Coordinates: 9:11\tType: " +
                                                      "StringLiteral\tSource Code: \'begin\'\tValue: \"\'begin\'\"");
            testLexer.SkipLexems(3);
            Exception e = Assert.Throws<Exception>(() => testLexer.NextLexem());
            Assert.AreEqual(e.Message,
                "TestPascalFiles\\StringLiterals.pas(10, 11) Fatal: String exceeds line");
        }
    }
}