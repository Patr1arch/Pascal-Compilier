using System.Collections.Generic;

namespace myPascal.Nodes
{
    public class CallNode : Node
    {
        public readonly List<Node> Args;
        private IdentNode _parent;

        public CallNode(IdentNode ident)
        {
            _parent = ident;
            Args = new List<Node>();
        }

        public override string Print(int depth = 0)
        {
            var res = _parent.Print(depth);
            foreach (var arg in Args)
            {
                res += arg.Print(depth + 1);
            }

            return res;
        }
    }
}