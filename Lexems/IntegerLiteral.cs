namespace myPascal.Lexems
{
    public class IntegerLiteral : AbstractLiteral
    {
        public IntegerLiteral(int strNum, int strSym) : base(strNum, strSym) {}
        
        public IntegerLiteral(AbstractLiteral literal) : 
            base(literal) {}
        
        public IntegerLiteral(AbstractLexem lexem) : 
            base(lexem) {}
    }
}
