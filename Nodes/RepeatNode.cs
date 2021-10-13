namespace myPascal.Nodes
{
    public class RepeatNode : Node
    {
        public Node UntilCondition { get; set; }
        public Node Body { get; set; }

        public RepeatNode(Node nd)
        {
            Body = nd;
        }

        public override string Print(int depth = 0)
        {
            return Body.Print(depth) + UntilCondition.Print(depth);
        }
    }
}