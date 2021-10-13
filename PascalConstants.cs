using System;
using System.Collections.Generic;

namespace myPascal
{
    public static class Pascal
    {
        public enum NumericTypes {Binary, Decimal, Hex, Real}

        public static char BinaryIdentifier = '%';
        
        public static char HexIdentifier = '$';

        public static string opMinus = "-";

        public static string opPlus = "+";

        public static string opMult = "*";

        public static string opIntDiv = "div";

        public static string opMod = "mod";

        public static string opDiv = "/";

        public static string lexLParent = "(";
        
        public static string lexRParent = ")";

        public static string programKey = "program";

        public static char sepStatement = ';';

        public static string opAssign = ":=";

        public static char sepComma = ',';

        public static string keyBegin = "begin";

        public static string keyEnd = "end";

        public static string keyWhile = "while";

        public static string keyDo = "do";

        public static string keyRepeat = "repeat";

        public static string keyUntil = "until";

        public static string keyFor = "for";

        public static string keyTo = "to";

        public static string keyDownto = "downto";

        public static List<char> Separators = new List<char>
        {
            '[',
            ']',
            sepStatement,
            '(',
            ')',
            ':',
            sepComma
        };

        public static List<string> RelationalOperators = new List<string>()
        {
            "=",
            "<>",
            "<",
            ">",
            "<=",
            ">=",
        };
        
        public static List<string> Operators = new List<string>
        {
            opAssign,
            opPlus, 
            opMinus,
            opMult,
            opDiv,
            "<<",
            ">>"
        };
        
        public enum Keyword
        {
            And,
            Or,
        }
        //Dictionary<
        public static List<string> Keywords = new List<string>
            {
                "and",
                "or",
                "boolean",
                "break",
                "byte",
                "continue",
                opDiv,
                "do",
                "double",
                "else",
                keyEnd,
                "false",
                "if",
                "int64",
                "longint",
                "longword",
                opMod,
                "not",
                "or",
                "qword",
                keyRepeat,
                "shl",
                "shortint",
                "shr",
                "single",
                "smallint",
                "then",
                "true",
                "uint64",
                keyUntil,
                keyWhile,
                "word",
                "xor",
                programKey,
                "integer",
                "real",
                "of",
                "var",
                keyBegin,
                "string",
                "procedure",
                "function",
                keyFor,
                keyTo,
                keyDownto
            };
    }
}