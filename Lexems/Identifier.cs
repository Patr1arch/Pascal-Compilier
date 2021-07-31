namespace myPascal.Lexems
{
    public class Identifier : AbstractIdentifier
    {
        public Identifier(int strNum, int symNum) : base(strNum, symNum)
        {
            Type = "Identifier";
        }
    }
}