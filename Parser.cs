using System;
using myPascal.Lexems;
using myPascal.Nodes;

namespace myPascal
{
    public class Parser
    {
        private Lexer _lex;

        public Parser(Lexer lex)
        {
            _lex = lex;
            _lex.NextLexem();
        }

        // Check if current token equal %lexemName
        // if yes, "eat" this lexem and promotes to next lexem calling NextLexem()
        // Otherwise throws exception
        public void Require(string lexemName)
        {
            if (_lex.GetLexem().Value.ToLower() != lexemName)
            {
                throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                    $"Fatal: Unexpected lexem {_lex.GetLexem().SourceCode}");
            }
            _lex.NextLexem();
        }
        
        public void Require(Type lexemType)
        {
            if (_lex.GetLexem().GetType() != lexemType)
            {
                throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                    $"Fatal: Unexpected lexem {_lex.GetLexem().SourceCode}");
            }
            _lex.NextLexem();
        }
        
        // Same logic but without throwing excepctions and return value
        public bool RequireWithoutThrows(string lexemName)
        {
            if (_lex.GetLexem().Value.ToLower() == lexemName)
            {
                _lex.NextLexem();
                return true;
            }

            return false;
        }
        

        public Node ParseProgram()
        {
            Require(Pascal.programKey);
            Require(typeof(Identifier));
            Require(Pascal.sepStatement.ToString());
            var block = ParseBlock();
            return block;
        }

        public Node ParseBlock()
        {
            // WIP: Without declaration part
            Require("begin");
            var stms = ParseStatementSequence();
            Require("end.");
            return stms;
        }

        public Node ParseStatementSequence()
        {
            var seq = new StatementSequence();
            do
            {
                if (_lex.GetLexem().Value == "end." || _lex.GetLexem().Value == Pascal.keyEnd || 
                    _lex.GetLexem().Value == Pascal.keyUntil)
                    break;
                seq.Statements.Add(ParseStatement());
            } while (RequireWithoutThrows(Pascal.sepStatement.ToString()));
            return seq;
        }

        public Node ParseStatement()
        {
            if (_lex.GetLexem() is Identifier)
            {
                var ident = new IdentNode(_lex.GetLexem());
                _lex.NextLexem();
                if (_lex.GetLexem().Value == Pascal.opAssign) // It's assign statement
                {
                    _lex.NextLexem();
                    return new AssignStatement(ident, ParseExpr());
                }
                else // It's also can be procedure identifier
                {
                    return ParseCallable(ident);
                }
            }

            if (_lex.GetLexem().Value == Pascal.keyBegin)
            {
                _lex.NextLexem();
                var stmt = ParseStatementSequence();
                Require(Pascal.keyEnd);
                return stmt;
            }

            if (_lex.GetLexem().Value == Pascal.keyWhile)
            {
                _lex.NextLexem();
                var whileNode = new WhileNode(ParseExpr());
                Require(Pascal.keyDo);
                whileNode.Body = ParseStatement();
                return whileNode;
            }

            if (_lex.GetLexem().Value == Pascal.keyRepeat)
            {
                _lex.NextLexem();
                var repeatNode = new RepeatNode(ParseStatementSequence()); // ParseStatementSequence() ?
                Require(Pascal.keyUntil);
                repeatNode.UntilCondition = ParseExpr();
                return repeatNode;
            }

            throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                $"Fatal: Unexpected lexem {_lex.GetLexem().Value}");
        }

        public Node ParseCallable(IdentNode ident)
        {
            var callNode = new CallNode(ident);
            if (_lex.GetLexem().Value == Pascal.lexLParent)
            {
                _lex.NextLexem();
                int count = 0;
                while (!RequireWithoutThrows(Pascal.lexRParent) && !_lex.IsEOFReached)
                {
                    if (count++ != 0)
                        Require(Pascal.sepComma.ToString());
                    var lexem = _lex.GetLexem();
                    if (lexem is Identifier)
                    {
                        _lex.NextLexem();
                        if (_lex.GetLexem().Value == Pascal.lexLParent)
                            callNode.Args.Add(ParseCallable(new IdentNode(lexem)));
                        else 
                            callNode.Args.Add(ParseExpr(new IdentNode(lexem)));
                    }
                    else
                    {
                        callNode.Args.Add(ParseExpr());
                    }
                }
            }
            
            return callNode;
        }

        public Node ParseExpr(Node nd = null)
        {
            Node left = nd ?? ParseTerm();
            AbstractLexem op = _lex.GetLexem();
            while (op.Value == Pascal.opPlus || op.Value == Pascal.opMinus)
            {
                _lex.NextLexem();
                Node right = ParseTerm();
                left = new BinOpNode(left, op, right);
                op = _lex.GetLexem();
            }
            
            AbstractLexem rop = _lex.GetLexem();
            if (Pascal.RelationalOperators.Contains(rop.Value))
            {
                _lex.NextLexem();
                return new BinOpNode(left, rop, ParseExpr());
            }

            return left;
        }

        public Node ParseTerm()
        {
            Node left = ParseFactor();
            AbstractLexem op = _lex.GetLexem();
            if (op.Value == Pascal.opMult || op.Value == Pascal.opDiv || op.Value == Pascal.opIntDiv || op.Value == Pascal.opMod)
            {
                _lex.NextLexem();
                Node right = ParseTerm();
                return new BinOpNode(left, op, right);
            }

            return left;
        }

        public Node ParseFactor()
        {
            var l = _lex.GetLexem();
            _lex.NextLexem();
            if (l is AbstractIdentifier) // true, false is keywords
            {
                _lex.GetLexem();
                if (_lex.GetLexem().Value == Pascal.lexLParent) // Callable Node
                    return ParseCallable(new IdentNode(l));
                return new IdentNode(l);
            }

            if (l is IntegerLiteral)
            {
                return new IntegerNode(l);
            }

            if (l is StringLiteral)
            {
                return new StringNode(l);
            }

            if (l.Value == Pascal.lexLParent)
            {
                Node e = ParseExpr();
                if (_lex.GetLexem().Value != Pascal.lexRParent) // Require
                {
                    throw new Exception($"{_lex.FilePath}{l.Coordinates} " +
                                        $"Fatal: Syntax error, \"{Pascal.lexRParent}\" expected but {_lex.GetLexem().SourceCode} found");
                }
                _lex.NextLexem();

                return e;
            }

            throw new Exception($"{_lex.FilePath}{l.Coordinates} Fatal: Unexpected lexem {l.Value}");
        }
    }
}
