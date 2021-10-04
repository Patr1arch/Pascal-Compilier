namespace myPascal.Lexems
{
    public class RealLiteral : AbstractLiteral
    {
        public RealLiteral(int strNum, int strSym) : base(strNum, strSym) {}
        
        public RealLiteral(AbstractLiteral literal) : 
            base(literal) {}
    }
}
