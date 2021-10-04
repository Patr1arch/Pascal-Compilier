namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static bool IsHex(this char sym)
        {
            return char.ToUpper(sym) >= 'A' && char.ToUpper(sym) <= 'F';
        }
        
        public static bool IsBinary(this char sym)
        {
            return sym == '0' || sym == '1';
        }
        
        public static bool IsFloat(this char sym)
        {
            return sym == '.' || sym == 'e' || sym == 'E';
        }
        
        public static bool IsDigitOrHexOrBinary(this char sym)
        {
            return char.IsDigit(sym) || IsBinary(sym) || IsHex(sym);
        }
        
        public static bool IsDigitOrHexOrBinaryOrFloat(this char sym)
        {
            return IsDigitOrHexOrBinary(sym) || IsFloat(sym);
        }


    }
}