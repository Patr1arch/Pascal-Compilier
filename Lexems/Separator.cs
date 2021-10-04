namespace myPascal.Lexems
{
    public class Separator : AbstractLexem
    {
        public Separator(int strNum, int symNum) : base(strNum, symNum) {}
        
        public Separator(AbstractLexem identifier) : base(identifier) {}
    }
}
