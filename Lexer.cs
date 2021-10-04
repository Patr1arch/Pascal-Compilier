using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using myPascal.Lexems;
using ExtensionMethods;

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

        private bool CheckForComment(char sym)
        {
            sym = CheckForWhitespaces(sym);
            if (sym == '{')
            {
                AbstractLexem comment = new AbstractLexem(_currentStringNumber, _currentSymbolNumber);
                while ((sym = (char) _stream.Read()) != '}') 
                {
                    if (sym == '\n')
                    {
                        _currentSymbolNumber = 1;
                        _currentStringNumber++;
                    }
                    else
                        _currentSymbolNumber++;
                }
                _buffer = sym;
                return true;
            }
            else if (sym == '/' && (sym = (char) _stream.Read()) == '/') // Cautions
            {
                while (sym != '\n')
                {
                    sym = (char) _stream.Read();
                }
                _currentSymbolNumber = 0;
                _currentStringNumber++;
                _buffer = sym;
                return true;
            }

            _buffer = sym;
            return false;
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
            if (CheckForComment(_buffer))
            {
                _currentSymbolNumber++;
                currSym = CheckForWhitespaces((char) _stream.Read());
            }
            

            if (char.IsDigit(currSym) || currSym == Pascal.BinaryIdentifier || currSym == Pascal.HexIdentifier)
            {
                AbstractLiteral literal = new AbstractLiteral(_currentStringNumber, _currentSymbolNumber);
                var detectedType = currSym == Pascal.BinaryIdentifier ? Pascal.NumericTypes.Binary :
                    currSym == Pascal.HexIdentifier ? Pascal.NumericTypes.Hex : Pascal.NumericTypes.Decimal;
                literal.SourceCode += currSym;
                // invariant: we get lexem and the next symbol, start with 2'd symbol of lexem
                while ((currSym = (char) _stream.Read()).IsDigitOrHexOrBinaryOrFloat())
                {
                    _buffer = currSym;
                    if (currSym.IsFloat() && detectedType != Pascal.NumericTypes.Hex)
                        detectedType = Pascal.NumericTypes.Real;
                    literal.SourceCode += _buffer;
                    _currentSymbolNumber++;
                }

                if (detectedType == Pascal.NumericTypes.Binary)
                {
                    literal.Value = Convert.ToInt64(literal.SourceCode.Substring(1), 2).ToString();
                }
                else if (detectedType == Pascal.NumericTypes.Hex)
                {
                    literal.Value = Convert.ToInt64(literal.SourceCode.Substring(1), 16).ToString();
                }
                else if (detectedType == Pascal.NumericTypes.Real)
                {
                    literal.Value = Convert.ToDouble(literal.SourceCode).ToString();
                }
                else
                {
                    literal.Value = Convert.ToInt64(literal.SourceCode, 10).ToString();
                }

                if (detectedType == Pascal.NumericTypes.Real)
                    _currentLexem = new RealLiteral(literal);
                else
                    _currentLexem = new IntegerLiteral(literal);
            }
            else if (char.IsLetter(currSym))
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
                    _currentSymbolNumber++;
                }
                if (Pascal.Keywords.Contains(identifier.SourceCode.ToLower()))
                {
                    _currentLexem = new Keyword(identifier);
                }
                else
                {
                    _currentLexem = new Identifier(identifier);
                }
            }
            else if (Pascal.Separators.Contains(currSym))
            {
                Separator separator = new Separator(_currentStringNumber, _currentSymbolNumber);
                _buffer = currSym;
                separator.Value += _buffer;
                separator.SourceCode += _buffer;
                if ((currSym = (char) _stream.Read()) == '=' && _buffer == ':')
                {
                    _currentLexem = new Operator(separator);
                }
                else
                {
                    _currentLexem = separator;
                }
            }

                _currentLexem = identifier;
            }

            _buffer = currSym;
            _currentSymbolNumber++;
        }

        public string GetLexemName()
        {
            return _currentLexem.ToString();
        }
    }
}