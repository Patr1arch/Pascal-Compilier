using System;
using myPascal.Lexems;

namespace myPascal.Nodes
{
    public class BinOpNode : Node
    {
        private Node _left, _right;

        private AbstractLexem _op; // op can be also mod or div keyword

        public BinOpNode(Node left, AbstractLexem op, Node right)
        {
            _left = left;
            _op = op;
            _right = right;
        }

        public override string Print(int depth = 0)
        {
            return base.Print(depth) + _op.Value + '\n' + _left.Print(depth + 1) + _right.Print(depth + 1);
        }
    }
}