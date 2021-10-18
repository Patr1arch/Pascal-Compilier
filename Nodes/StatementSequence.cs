using System.Collections.Generic;

namespace myPascal.Nodes
{
    public class StatementSequence : Node
    {
        public readonly List<Node> Statements;

        public StatementSequence()
        {
            Statements = new List<Node>();
        }

        public override string Print(int depth = 0)
        {
            var res = "";
            foreach (Node nd in Statements)
                res += nd.Print(depth);
            return res;
        }
    }
}