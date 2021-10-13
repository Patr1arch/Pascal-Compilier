using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class ForNode : Node
    {
        public IdentNode Ident { get; set; }
        public bool IsTo { get; set; }
        public Node InitialExpr { get; set; }
        public Node FinalExpr { get; set; }
        public Node Body { get; set; }

        public ForNode(AbstractLexem ident)
        {
            Ident = new IdentNode(ident);
        }

        public override string Print(int depth = 0)
        {
            return Ident.Print(depth) + InitialExpr.Print(depth) + FinalExpr.Print(depth) + Body.Print(depth);
        }
    }
}