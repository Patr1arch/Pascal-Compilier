namespace myPascal.Lexems
{
    public class Operator : AbstractLexem
    {
        public Operator(int strNum, int symNum) : base(strNum, symNum) {}
        
        public Operator(AbstractLexem identifier) : base(identifier) {}
    }
}
