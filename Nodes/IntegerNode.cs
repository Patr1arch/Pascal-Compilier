using System;
using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class IntegerNode : Node
    {
        public IntegerLiteral Lexem { get; set; }

        public int Value => Int32.Parse(Lexem.Value);

        public IntegerNode(AbstractLexem lex) // Not sure about IntLit in param
        {
            Lexem = new IntegerLiteral(lex);
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + Value + '\n';
        }
    }
}