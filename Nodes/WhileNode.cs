using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class WhileNode : Node
    {
        public Node Condition { get; set; }
        public Node Body { get; set; }

        public WhileNode(Node nd)
        {
            Condition = nd;
        }

        public override string Print(int depth = 0) 
        {
            return base.Print(depth) + "while\n" + Condition.Print(depth + 1) + 
                   base.Print(depth) + "do\n" + Body.Print(depth + 1);
        }
    }
}