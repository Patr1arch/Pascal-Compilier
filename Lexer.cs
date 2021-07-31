﻿using System;
using System.Collections.Generic;
using System.IO;
using myPascal.Lexems;

namespace myPascal
{
    public class Lexer
    {
        private readonly StreamReader _stream; // File?
        private char _buffer;
        private readonly int _currentStringNumber;
        private int _currentSymbolNumber;
        private AbstractLexem _currentLexem;
        public Lexer(string filePath)
        {
            _stream = new StreamReader(filePath);
            _currentStringNumber = 1;
            _currentSymbolNumber = 0;
        }

        public List<AbstractLexem> GetAllLexems()
        {
            var res = new List<AbstractLexem>();
            return res;
        }

        public void NextLexem() 
        {
            char currSym = (char) _stream.Read();
            _buffer = currSym;
            _currentSymbolNumber++;
            
            if (char.IsLetter(currSym))
            {
                Identifier identifier = new Identifier(_currentStringNumber, _currentSymbolNumber);
                identifier.Value += currSym;
                identifier.SourceCode += currSym;
                // invariant: we get lexem and the next symbol
                while(char.IsLetterOrDigit(currSym = (char)_stream.Read()))
                {
                    _buffer = currSym;
                    identifier.Value += _buffer;
                    identifier.SourceCode += _buffer;
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