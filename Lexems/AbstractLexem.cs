namespace myPascal.Lexems
{
    public class AbstractLexem
    {
        public (int, int) Coordinates { get; set; }
        public string SourceCode { get; set; }
        public string Value { get; set; }
        public AbstractLexem(int strNum, int symNum)
        {
            Coordinates = (strNum, symNum);
        }

        public override string ToString()
        {
            return $"Coordinates: {Coordinates.Item1}:{Coordinates.Item2}\t" +
                   $"Type: {GetType().Name}\t" +
                   $"Source Code: {SourceCode}\t" +
                   $"Value: \"{Value}\"";
        }
    }
}
