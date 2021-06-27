namespace myPascal.Tokenizer
{
    public class Lexer
    {
        private string filePath; // File?

        public Lexer(string filePath)
        {
            this.filePath = filePath;
        }

        public char NextToken()
        {
            return 'a';
        }

        public string GetTokenName()
        {
            return "todo";
        }
    }
}