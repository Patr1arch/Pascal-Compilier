using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class RealNode : Node
    {
        private RealLiteral _realLiteral;

        public RealNode(AbstractLexem lex)
        {
            _realLiteral = new RealLiteral(lex);
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + _realLiteral.SourceCode + '\n';
        }
    }
}