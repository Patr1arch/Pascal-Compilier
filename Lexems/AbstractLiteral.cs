namespace myPascal.Lexems
{
    public class AbstractLiteral : AbstractLexem
    {
        public AbstractLiteral(int strNum, int strSym) : base(strNum, strSym)
        {
        }
        
        public AbstractLiteral(AbstractLexem lexem) : base(lexem)
        {
        }
    }
}
