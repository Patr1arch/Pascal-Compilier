using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using myPascal.Lexems;
using System.Globalization;
using ExtensionMethods;

namespace myPascal
{
    public class Lexer
    {
        private StreamReader _stream; // File?
        private char _buffer;
        private int _currentStringNumber;
        private int _currentSymbolNumber;
        private AbstractLexem _currentLexem;
        private string _filePath;

        public string FilePath => _filePath;
        public Lexer(string filePath = "")
        {
            _stream = filePath != "" ? new StreamReader(filePath) : null;
            _currentStringNumber = 1;
            _currentSymbolNumber = 0;
            _buffer = ' ';
            _filePath = filePath;
        }

        public void SetFilePath(string filePath)
        {
            _stream = new StreamReader(filePath);
            _currentStringNumber = 1;
            _currentSymbolNumber = 0;
            _buffer = ' ';
            _filePath = filePath;
        }
        
        public void SkipLexems(int count)
        {
            for (int i = 0; i < count; i++)
            {
                NextLexem();
            }
        }

        public List<AbstractLexem> GetAllLexems()
        {
            var res = new List<AbstractLexem>();
            // invariant: we reach end of file, but we don't proceed symbol previous EOF
            while (!_stream.EndOfStream)
            {
                NextLexem();
                if (res.Count == 0 || res.Last() != _currentLexem)
                    res.Add(_currentLexem);
            }

            if (!char.IsWhiteSpace(CheckForWhitespaces(_buffer)))
            {
                NextLexem();
                res.Add(_currentLexem);
            }

            return res;
        }

        private bool CheckForComment(char sym)
        {
            sym = CheckForWhitespaces(sym);
            AbstractLexem comment = new AbstractLexem(_currentStringNumber, _currentSymbolNumber);
            if (sym == '{')
            {
                while ((sym = (char) _stream.Read()) != '}') 
                {
                    if (_stream.EndOfStream)
                    {
                        throw new Exception($"{_filePath}{comment.Coordinates} Fatal: Detected unclosed comment");
                    }
                    if (sym == '\n')
                    {
                        _currentSymbolNumber = 1;
                        _currentStringNumber++;
                    }
                    else
                        _currentSymbolNumber++;
                }

                sym = (char) _stream.Read(); // Eat }
                _currentSymbolNumber++;
                _buffer = sym;
                return CheckForComment(sym);
            }
            else if (sym == '/' && (sym = (char) _stream.Read()) == '/') // Cautions
            {
                while (sym != '\n' && !_stream.EndOfStream)
                {
                    sym = (char) _stream.Read();
                }
                // To satisfy invariant
                sym = (char) _stream.Read();
                _currentSymbolNumber = 1;
                _currentStringNumber++;
                _buffer = sym;
                return CheckForComment(sym);    
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
            CheckForComment(_buffer);
            var currSym = _buffer;
            
            if (currSym == '}')
                throw new Exception($"{_filePath}{(_currentStringNumber, _currentSymbolNumber)}" +
                                    $" Fatal: Detected invalid character");
            
            if (char.IsDigit(currSym) || currSym == Pascal.BinaryIdentifier || currSym == Pascal.HexIdentifier)
            {
                AbstractLiteral literal = new AbstractLiteral(_currentStringNumber, _currentSymbolNumber);
                var detectedType = currSym == Pascal.BinaryIdentifier ? Pascal.NumericTypes.Binary :
                    currSym == Pascal.HexIdentifier ? Pascal.NumericTypes.Hex : Pascal.NumericTypes.Decimal;
                literal.SourceCode += currSym;
                // invariant: we get lexem and the next symbol, start with 2nd symbol of lexem
                while ((currSym = (char) _stream.Read()).IsDigitOrHexOrBinaryOrFloat() ||
                       currSym.ToString() == Pascal.opMinus && detectedType == Pascal.NumericTypes.Real)
                {
                    _buffer = currSym;
                    if (currSym.IsFloat() && detectedType != Pascal.NumericTypes.Hex)
                    {
                        if ((char) _stream.Peek() == '.') // detected number and slice operator ..
                            break;
                        detectedType = Pascal.NumericTypes.Real;
                    }
                    literal.SourceCode += _buffer;
                    _currentSymbolNumber++;
                }

                switch (detectedType)
                {
                    case Pascal.NumericTypes.Binary:
                        literal.Value = Convert.ToInt64(literal.SourceCode.Substring(1), 2).ToString();
                        break;
                    case Pascal.NumericTypes.Hex:
                        literal.Value = Convert.ToInt64(literal.SourceCode.Substring(1), 16).ToString();
                        break;
                    case Pascal.NumericTypes.Real:
                        literal.Value = double.Parse(literal.SourceCode, CultureInfo.InvariantCulture).ToString();
                        break;
                    default:
                        literal.Value = Convert.ToInt64(literal.SourceCode, 10).ToString();
                        break;
                }

                if (detectedType == Pascal.NumericTypes.Real)
                    _currentLexem = new RealLiteral(literal);
                else
                    _currentLexem = new IntegerLiteral(literal);
            }
            else if (char.IsLetter(currSym) || currSym == Pascal.lexUnderscore)
            {
                AbstractIdentifier identifier = new AbstractIdentifier(_currentStringNumber, _currentSymbolNumber);
                identifier.Value += currSym;
                identifier.SourceCode += currSym;
                // invariant: we get lexem and the next symbol
                while(char.IsLetterOrDigit(currSym = (char)_stream.Read()) || currSym == Pascal.lexUnderscore)
                {
                    _buffer = currSym;
                    identifier.Value += _buffer;
                    identifier.SourceCode += _buffer;
                    _currentSymbolNumber++;
                }

                if (identifier.SourceCode.Length > 255)
                {
                    throw new Exception($"{_filePath}{identifier.Coordinates} " +
                                        $"Fatal: Length of identifier more than 255");
                }

                if (Pascal.Keywords.Contains(identifier.SourceCode.ToLower()))
                {
                    if (currSym == '.' && identifier.SourceCode.ToLower() == "end")
                    {
                        identifier.SourceCode += currSym;
                        identifier.Value += currSym;
                        
                        // To satisfy common invarant
                        currSym = (char) _stream.Read();
                    }
                    _currentLexem = new Keyword(identifier);
                }
                else
                {
                    _currentLexem = new Identifier(identifier);
                }
            }
            else if (currSym == Pascal.apostrophe)
            {
                StringLiteral stringLiteral = HandleQuotedString();
                stringLiteral.Value = Pascal.apostrophe + stringLiteral.Value + Pascal.apostrophe;
                currSym = _buffer;
                _currentLexem = stringLiteral;
            }
            else if (currSym == Pascal.hash)
            {
                var lexem = HandleControlString();
                lexem.Value = Pascal.apostrophe + lexem.Value + Pascal.apostrophe;
                currSym = _buffer;
                _currentLexem = lexem;
            }
            else if (Pascal.Separators.Contains(currSym))
            {
                Separator separator = new Separator(_currentStringNumber, _currentSymbolNumber);
                _buffer = currSym;
                separator.Value += _buffer;
                separator.SourceCode += _buffer;
                if ((currSym = (char) _stream.Read()) == '=' && _buffer == ':' ||
                    _buffer == Pascal.sepDot && currSym == Pascal.sepDot)
                {
                    separator.Value += currSym;
                    separator.SourceCode += currSym;
                    _currentSymbolNumber++; // Operator of two symbols
                    _currentLexem = new Operator(separator);
                    // To satisfy common invarant
                    currSym = (char) _stream.Read();
                }
                else
                {
                    _currentLexem = separator;
                }
            }
            else if (Pascal.Operators.Contains(currSym.ToString()) || 
                     Pascal.RelationalOperators.Contains(currSym.ToString()) ||
                     currSym == '.')
            {
                Operator @operator = new Operator(_currentStringNumber, _currentSymbolNumber);
                _buffer = currSym;
                @operator.Value += _buffer;
                @operator.SourceCode += _buffer;
                
                if (Pascal.Operators.Contains(@operator.Value + (currSym = (char) _stream.Read()).ToString()) ||
                    Pascal.RelationalOperators.Contains(@operator.Value + currSym.ToString()))
                {
                    @operator.Value += currSym;
                    @operator.SourceCode += currSym;
                    _currentSymbolNumber++; // Operator of two symbols
                    _currentLexem = @operator;
                
                    // To satisfy common invarant
                    currSym = (char) _stream.Read();
                }
                else
                {
                    _currentLexem = @operator;
                }
                
            }

            _buffer = currSym;
            _currentSymbolNumber++;
        }

        public StringLiteral HandleQuotedString()
        {
            var stringLiteral = new StringLiteral(_currentStringNumber, _currentSymbolNumber);
            stringLiteral.SourceCode += "\'"; // TODO Redefine setters for SourceCode/Value except Integer and Real
            // invariant: we get lexem/word and next symbol
            var currSym = _buffer;
            while ((currSym = (char) _stream.Read()) != '\n' && currSym != Pascal.endOfFile)
            {
                if (currSym == Pascal.apostrophe)
                {
                    _buffer = currSym;
                    _currentSymbolNumber++;
                    stringLiteral.SourceCode += Pascal.apostrophe;
                    if (_stream.Peek() == Pascal.hash)
                    {
                        _stream.Read();
                        _currentSymbolNumber++;
                        stringLiteral.Combine(HandleControlString());
                        break;
                    }
                    else if (_stream.Peek() == Pascal.apostrophe)
                    {
                        stringLiteral.Value += Pascal.apostrophe;
                        stringLiteral.SourceCode += Pascal.apostrophe;
                        _currentSymbolNumber++;
                        _stream.Read();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                _buffer = currSym;
                stringLiteral.Value += _buffer;
                stringLiteral.SourceCode += _buffer;
                _currentSymbolNumber++;
            }
            if (currSym == '\n')
                throw new Exception($"{_filePath}{stringLiteral.Coordinates} " +
                                    $"Fatal: String exceeds line");

            // To satisfy common invariant
            currSym = (char) _stream.Read(); // TODO: Optimize this calls if possible

            _buffer = currSym;
            return stringLiteral;
        }

        public StringLiteral HandleControlString()
        {
            StringLiteral lexem = new StringLiteral(_currentStringNumber, _currentSymbolNumber);
            lexem.SourceCode += Pascal.hash.ToString();
            var currSym = _buffer;
            while (char.IsDigit(currSym = (char) _stream.Read()))
            {
                _buffer = currSym;
                lexem.SourceCode += _buffer;
                _currentSymbolNumber++;
            }
            
            if (lexem.SourceCode.Length == 1) // Only # without numbers
                throw new Exception($"{_filePath}{lexem.Coordinates} " +
                                    $"Fatal: '#' lexem must has integer value");
            lexem.Value = Convert.ToChar(Convert.ToInt16(lexem.SourceCode.Substring(1))).ToString();

            _buffer = currSym;
            
            if (currSym == Pascal.apostrophe)
            {
                lexem.Combine(HandleQuotedString());
            }

            if (currSym == Pascal.hash)
            {
                lexem.Combine(HandleControlString());
            }
            
            return lexem;
        }

        public string GetLexemName()
        {
            return _currentLexem.ToString();
        }

        public AbstractLexem GetLexem()
        {
            return _currentLexem;
        }

        public AbstractLexem GetNextLexem()
        {
            NextLexem();
            return GetLexem();
        }

        public bool IsEOFReached => _stream.EndOfStream;
    }
}