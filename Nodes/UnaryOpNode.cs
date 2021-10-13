using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class UnaryOpNode : Node
    {
        public Node Value { get; set; }
        public AbstractLexem UnOp { get; set; }

        public UnaryOpNode(AbstractLexem op, Node value)
        {
            UnOp = op;
            Value = value;
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + UnOp.Value + '\n' + Value.Print(depth + 1);
        }
    }
}