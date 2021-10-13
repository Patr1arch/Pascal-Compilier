using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class RecordFieldAccessNode : Node
    {
        public IdentNode Parent { get; set; }
        
        public Node Child { get; set; }

        public RecordFieldAccessNode(IdentNode parent, Node child)
        {
            Parent = parent;
            Child = child;
        }

        public override string Print(int depth = 0)
        {
            return Parent.Print(depth) + Child.Print(depth + 1);
        }
    }
}