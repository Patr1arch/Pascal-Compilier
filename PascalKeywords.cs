using System;
using System.Collections.Generic;

namespace myPascal
{
    public static class Pascal
    {
        public enum NumericTypes {Binary, Decimal, Hex, Real}

        public static char BinaryIdentifier = '%';
        
        public static char HexIdentifier = '$';

        public static List<char> Separators = new List<char>
        {
            '[',
            ']',
            ';',
            '(',
            ')',
            ':',
            ',',
            '"'
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
            "+",
            "-",
            "*",
            "/",
            "<<",
            ">>"
        };
        
        public static List<string> Keywords = new List<string>
            {
                "and",
                "or",
                "boolean",
                "break",
                "byte",
                "continue",
                "div",
                "do",
                "double",
                "else",
                "end",
                "false",
                "if",
                "int64",
                "longint",
                "longword",
                "mod",
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
                "while",
                "word",
                "xor",
                "program",
                "integer",
                "real",
                "of",
                "var",
                "begin",
                "string"
            };
    }
}