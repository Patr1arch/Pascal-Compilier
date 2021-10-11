namespace myPascal.Nodes
{
    public class AssignStatement : Node
    {
        private IdentNode _left;
        private Node _right;

        public AssignStatement(IdentNode left, Node exprNode)
        {
            _left = left;
            _right = exprNode;
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + ":=\n" + _left.Print(depth + 1) + _right.Print(depth + 1);
        }
    }
}