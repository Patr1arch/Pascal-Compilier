using System;
using myPascal.Lexems;
using myPascal.Nodes;

namespace myPascal
{
    public class Parser
    {
        private Lexer _lex;

        public Parser(Lexer lex)
        {
            _lex = lex;
            _lex.NextLexem();
        }

        public Node ParseExpr()
        {
            Node left = ParseTerm();
            AbstractLexem op = _lex.GetLexem();
            while (op.Value == Pascal.opPlus || op.Value == Pascal.opMinus)
            {
                _lex.NextLexem();
                Node right = ParseTerm();
                left = new BinOpNode(left, op, right);
                op = _lex.GetLexem();
            }

            return left;
        }

        public Node ParseTerm()
        {
            Node left = ParseFactor();
            AbstractLexem op = _lex.GetLexem();
            if (op.Value == Pascal.opMult || op.Value == Pascal.opDiv || op.Value == Pascal.opIntDiv || op.Value == Pascal.opMod)
            {
                _lex.NextLexem();
                Node right = ParseTerm();
                return new BinOpNode(left, op, right);
            }

            return left;
        }

        public Node ParseFactor()
        {
            var l = _lex.GetLexem();
            _lex.NextLexem();
            if (l.GetType() == typeof(Identifier))
            {
                return new IdentNode(l);
            }

            if (l.GetType() == typeof(IntegerLiteral))
            {
                return new IntegerNode(l);
            }

            if (l.Value == Pascal.lexLParent)
            {
                Node e = ParseExpr();
                if (_lex.GetLexem().Value != Pascal.lexRParent)
                {
                    throw new Exception(_lex.GetLexemName());
                }
                _lex.NextLexem();

                return e;
            }

            throw new Exception($"{_lex.FilePath}{l.Coordinates} Fatal: Unexpected lexem {l.Value}");
        }
    }
}
