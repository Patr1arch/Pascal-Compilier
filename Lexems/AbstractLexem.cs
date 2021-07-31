namespace myPascal.Lexems
{
    public class AbstractLexem
    {
        public (int, int) Coordinates { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string SourceCode { get; set; }
        protected AbstractLexem(int strNum, int symNum)
        {
            Coordinates = (strNum, symNum);
        }

        public override string ToString()
        {
            return $"Coordinates: {Coordinates.Item1}:{Coordinates.Item2}\tType: {Type}\tSource Code: {SourceCode}\t" +
                   $"Value: \"{Value}\"";
        }
    }
}