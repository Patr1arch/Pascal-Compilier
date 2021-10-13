using System;
using System.Collections.Generic;
using myPascal.Lexems;

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

        public static string keyIf = "if";

        public static string keyThen = "then";
        
        public static string keyElse = "else";

        public static string keyCase = "case";

        public static string keyOf = "of";

        public static char sepColon = ':';

        public static string keyNil = "nil";
        
        public static string keyNot = "not";

        public static string keyIn = "in";

        public static string keyOr = "or";

        public static string keyAnd = "and";

        public static List<char> Separators = new List<char>
        {
            '[',
            ']',
            sepStatement,
            '(',
            ')',
            sepColon,
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
                keyAnd,
                keyOr,
                "boolean",
                "break",
                "byte",
                "continue",
                opDiv,
                "do",
                "double",
                keyElse,
                keyEnd,
                "false",
                keyIf,
                "int64",
                "longint",
                "longword",
                opMod,
                keyNot,
                "or",
                "qword",
                keyRepeat,
                "shl",
                "shortint",
                "shr",
                "single",
                "smallint",
                keyThen,
                "true",
                "uint64",
                keyUntil,
                keyWhile,
                "word",
                "xor",
                programKey,
                "integer",
                "real",
                keyOf,
                "var",
                keyBegin,
                "string",
                "procedure",
                "function",
                keyFor,
                keyTo,
                keyDownto,
                keyCase,
                keyNil,
                keyIn
            };
    }
}