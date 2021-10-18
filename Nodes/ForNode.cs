using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class ForNode : Node
    {
        public Node Ident { get; set; }
        public bool IsTo { get; set; }
        public Node InitialExpr { get; set; }
        public Node FinalExpr { get; set; }
        public Node Body { get; set; }

        public ForNode(Node var)
        {
            Ident = var;
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + "for\n" + base.Print(depth + 1) + ":=\n" + Ident.Print(depth + 2) + 
                   InitialExpr.Print(depth + 2) + base.Print(depth + 1) +
                   (IsTo ? "to" : "downto") + "\n" + FinalExpr.Print(depth + 2) + 
                   base.Print(depth + 1) + "do\n" + Body.Print(depth + 2);
        }
    }
}