namespace myPascal.Nodes
{
    public class IfNode : Node
    {
        public Node Condition { get; set; }
        public Node ThenStmt { get; set; }
        public Node ElseStmt { get; set; }

        public IfNode(Node cnd)
        {
            Condition = cnd;
            ElseStmt = new EmptyStatement();
        }

        public override string Print(int depth = 0)
        {
            return Condition.Print(depth) + ThenStmt.Print(depth) + ElseStmt.Print(depth);
        }
    }
}