namespace myPascal.Nodes
{
    public class PointerNode : Node
    {
        public Node Ref { get; set; }

        public PointerNode(Node @ref)
        {
            Ref = @ref;
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + "^\n" + Ref.Print(depth + 1);
        }
    }
}