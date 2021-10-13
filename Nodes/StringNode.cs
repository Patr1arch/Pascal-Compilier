using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class StringNode : Node
    {
        private StringLiteral _lex;

        public string Name => _lex.Value;

        public StringNode(AbstractLexem str)
        {
            _lex = new StringLiteral(str);
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + Name + '\n';
        }
    }
}