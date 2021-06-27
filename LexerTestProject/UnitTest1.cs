//using Microsoft.VisualStudio.TestPlatform.TestHost;

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
        public void Test1()
        {
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"-l", "test.pas"}), 0);
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"test.pas"}), 0);
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"-l", "testtrash.pas"}), 1);
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"testtrash.pas"}), 1);
            // TODO: Handle this problem if you'll have free time
            Assert.AreEqual(Program.ProceedArguments(new List<string>(){"-aba", "testtrash.pas"}), 1);
        }
    }
}