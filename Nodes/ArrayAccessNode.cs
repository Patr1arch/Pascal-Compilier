using System.Collections.Generic;
using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class ArrayAccessNode : Node
    {
        public Node Array { get; set; }
        public List<Node> Indexes { get; set; }
        public ArrayAccessNode(Node ident)
        {
            Array = ident;
            Indexes = new List<Node>();
        }

        public override string Print(int depth = 0)
        {
            var res = Array.Print(depth);
            foreach (var index in Indexes)
            {
                res += index.Print(depth + 1);
            }

            return res;
        }
    }
}