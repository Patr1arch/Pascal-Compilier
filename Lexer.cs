using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using myPascal.Lexems;

namespace myPascal
{
    public class Lexer
    {
        private readonly StreamReader _stream; // File?
        private char _buffer;
        private int _currentStringNumber;
        private int _currentSymbolNumber;
        private AbstractLexem _currentLexem;
        public Lexer(string filePath)
        {
            _stream = new StreamReader(filePath);
            _currentStringNumber = 1;
            _currentSymbolNumber = 0;
            _buffer = ' ';
        }

        public List<AbstractLexem> GetAllLexems()
        {
            var res = new List<AbstractLexem>();
            return res;
        }

        private char CheckForWhitespaces(char sym)
        {
            while (char.IsWhiteSpace(sym))
            {
                if (sym == '\n')
                {
                    _currentSymbolNumber = 1;
                    _currentStringNumber++;
                }
                else
                    _currentSymbolNumber++;

                sym = (char) _stream.Read();
                _buffer = sym;
            }

            return sym;
        }

        public void NextLexem()
        {
            // EOF -- will be fine, 'cause 56635 is not valid symbol in unicode table 
            char currSym = CheckForWhitespaces(_buffer);
            _buffer = currSym;
            _currentSymbolNumber++;
            
            if (char.IsLetter(currSym))
            {
                AbstractIdentifier identifier = new AbstractIdentifier(_currentStringNumber, _currentSymbolNumber);
                identifier.Value += currSym;
                identifier.SourceCode += currSym;
                // invariant: we get lexem and the next symbol
                while(char.IsLetterOrDigit(currSym = (char)_stream.Read()))
                {
                    _buffer = currSym;
                    identifier.Value += _buffer;
                    identifier.SourceCode += _buffer;
                if (Pascal.Keywords.Contains(identifier.SourceCode.ToLower()))
                {
                    _currentLexem = new Keyword(identifier);
                }
                else
                {
                    _currentLexem = new Identifier(identifier);
                }
            }

                _currentLexem = identifier;
            }
        }

        public string GetLexemName()
        {
            return _currentLexem.ToString();
        }
    }
}