using System.Collections.Generic;

namespace myPascal.Nodes
{
    public class CaseNode : Node
    {
        public Node Expr { get; set; }
        public List<(List<Node>, Node)> Cases { get; set; }

        public CaseNode(Node expr)
        {
            Expr = expr;
            Cases = new List<(List<Node>, Node)>();
        }

        public override string Print(int depth = 0)
        {
            var res = Expr.Print();
            foreach (var @case in Cases)
            {
                var subRes = "";
                foreach (var label in @case.Item1)
                {
                    subRes += label.Print(depth + 1);
                }

                res += subRes + @case.Item2.Print(depth + 2);
            }

            return res;
        }
    }
}