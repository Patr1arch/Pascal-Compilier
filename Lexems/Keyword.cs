namespace myPascal.Lexems
{
    public class Keyword : AbstractIdentifier
    {
        public Keyword(int strNum, int symNum) : base(strNum, symNum)
        {
            Type = "Keyword";
        }
    }
}