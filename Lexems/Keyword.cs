namespace myPascal.Lexems
{
    public class Keyword : AbstractIdentifier
    {
        public Keyword(int strNum, int symNum) : base(strNum, symNum)
        {
        }

        public Keyword(AbstractIdentifier identifier) : 
            base(identifier) {}
    }
}
