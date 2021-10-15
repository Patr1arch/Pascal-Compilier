namespace myPascal.Lexems
{
    public class StringLiteral : AbstractLiteral
    {
        public StringLiteral(int strNum, int strSym) : base(strNum, strSym) {}

        public StringLiteral(AbstractLexem lexem) : base(lexem) {}

        public void Combine(StringLiteral lt)
        {
            SourceCode += lt.SourceCode;
            Value += lt.Value;
        }
    }
}