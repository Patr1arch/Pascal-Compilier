using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class NilNode : Node
    {
        public AbstractLexem NilLex { get; set; }

        public NilNode(AbstractLexem nilLex)
        {
            NilLex = nilLex;
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + NilLex.Value + '\n';
        }
    }
}