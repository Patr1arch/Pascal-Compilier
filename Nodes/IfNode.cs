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
            return base.Print(depth) + "if\n" + Condition.Print(depth + 1)
                   + base.Print(depth) + "then\n" + ThenStmt.Print(depth + 1) + 
                   (ElseStmt.Print(depth + 1) != "" ? base.Print(depth) + "else\n" + ElseStmt.Print(depth + 1) : "");
        }
    }
}