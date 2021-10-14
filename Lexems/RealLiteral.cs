namespace myPascal.Lexems
{
    public class RealLiteral : AbstractLiteral
    {
        public RealLiteral(int strNum, int strSym) : base(strNum, strSym) {}
        
        public RealLiteral(AbstractLexem literal) : 
            base(literal) {}
    }
}
