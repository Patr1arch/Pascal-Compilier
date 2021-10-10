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

        public static List<char> Separators = new List<char>
        {
            '[',
            ']',
            ';',
            '(',
            ')',
            ':',
            ','
        };
        
        public static List<string> Operators = new List<string>
        {
            ":=",
            "=",
            "<>",
            "<",
            ">",
            "<=",
            ">=",
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
                "end",
                "false",
                "if",
                "int64",
                "longint",
                "longword",
                opMod,
                "not",
                "or",
                "qword",
                "repeat",
                "shl",
                "shortint",
                "shr",
                "single",
                "smallint",
                "then",
                "true",
                "uint64",
                "until",
                "while",
                "word",
                "xor",
                "program",
                "integer",
                "real",
                "of",
                "var",
                "begin",
                "string",
                "procedure",
                "function"
            };
    }
}