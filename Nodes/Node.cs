using System;

namespace myPascal.Nodes
{
    public class Node
    {
        public virtual string Print(int depth = 0)
        {
            string res = "";
            for (int i = 0; i < depth; i++)
            {
                res += '\t';
            }

            return res;
        }
    }
}