using System;
using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class IdentNode : Node
    {
        private Identifier _lex;

        public string Name => _lex.Value;

        public IdentNode(AbstractLexem ident)
        {
            _lex = new Identifier(ident);
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + Name + '\n';
        }
    }
}