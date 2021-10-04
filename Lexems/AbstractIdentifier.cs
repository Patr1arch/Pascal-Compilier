namespace myPascal.Lexems
{
    public class AbstractIdentifier : AbstractLexem
    {
        public AbstractIdentifier(int strNum, int symNum) : base(strNum, symNum)
        {
        }
        
        public AbstractIdentifier(AbstractLexem lexem) : base(lexem)
        {
        }
    }
}
