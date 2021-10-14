using System;
using System.Collections.Generic;
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
            // TODO: What about _ ?
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
            Require(Pascal.sepSemicolon.ToString());
            var block = ParseBlock();
            return block;
        }

        public Node ParseBlock()
        {
            // WIP: Without declaration part
            Require(Pascal.keyBegin);
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
            } while (RequireWithoutThrows(Pascal.sepSemicolon.ToString()));
            return seq;
        }
        
        public Node ParseVariable(IdentNode nd = null)
        {
            IdentNode ident = null;
            
            if (nd != null)
            {
                ident = nd;
            }
            else
            {
                ident = new IdentNode(_lex.GetLexem());
                _lex.NextLexem();
            }
            Node specialNode = null;
            if (_lex.GetLexem().Value == Pascal.sepLBracket.ToString())
            {
                specialNode = ParseArrayAccess(ident);
            }

            if (_lex.GetLexem().Value == Pascal.opCaret)
            {
                _lex.NextLexem();
                if (_lex.GetLexem().Value == Pascal.sepDot.ToString())
                {
                    _lex.NextLexem();
                    if (_lex.GetLexem() is Identifier)
                        return  new RecordFieldAccessNode(new PointerNode(ident), ParseVariable());
                    else throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                             $"Fatal: Expected record field identifier");
                }
                return new PointerNode(specialNode ?? ident);
            }
            
            if (_lex.GetLexem().Value == Pascal.sepDot.ToString())
            {
                _lex.NextLexem();
                if (_lex.GetLexem() is Identifier)
                    return new RecordFieldAccessNode(ident, ParseVariable());
                else throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                         $"Fatal: Expected record field identifier");
            }

            return specialNode ?? ident;
        }

        public Node ParseStatement()
        {
            if (_lex.GetLexem() is Identifier)
            {
                Node ident = ParseVariable();
                if (_lex.GetLexem().Value == Pascal.opAssign) // It's assign statement
                {
                    _lex.NextLexem();
                    //if (_lex.GetLexem().Value == Pascal.sepLBracket.ToString())
                    //    return new AssignStatement(ParseArrayAccess(ident), ParseExpr());
                    return new AssignStatement(ident, ParseExpr());
                }
                else
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

            if (_lex.GetLexem().Value == Pascal.keyFor)
            {
                _lex.NextLexem();
                if (_lex.GetLexem() is Identifier)
                {
                    var forNode = new ForNode(_lex.GetLexem());
                    _lex.NextLexem();
                    Require(Pascal.opAssign);
                    forNode.InitialExpr = ParseExpr();
                    if (_lex.GetLexem().Value == Pascal.keyTo)
                        forNode.IsTo = true;
                    else if (_lex.GetLexem().Value == Pascal.keyDownto)
                        forNode.IsTo = false;
                    else throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                                   $"Fatal: Syntax error, \"{Pascal.lexRParent}\" expected but " +
                                                   $"{_lex.GetLexem().SourceCode} found");
                    _lex.NextLexem();
                    forNode.FinalExpr = ParseExpr();
                    Require(Pascal.keyDo);
                    forNode.Body = ParseStatement();
                    return forNode;
                }
                else
                    throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                    $"Fatal: Expected variable identifier");
            }

            if (_lex.GetLexem().Value == Pascal.keyIf)
            {
                _lex.NextLexem();
                var ifNode = new IfNode(ParseExpr());
                Require(Pascal.keyThen);
                ifNode.ThenStmt = ParseStatement();
                if (_lex.GetLexem().Value == Pascal.keyElse)
                {
                    _lex.NextLexem();
                    ifNode.ElseStmt = ParseStatement();
                }

                return ifNode;
            }

            if (_lex.GetLexem().Value == Pascal.keyCase)
            {
                _lex.NextLexem();
                var caseNode = new CaseNode(ParseExpr());
                Require(Pascal.keyOf);
                do
                {
                    var cnst = _lex.GetLexem();
                    var caseLabelList = new List<Node>();
                    while (cnst.Value != Pascal.sepColon.ToString())
                    {
                        if (cnst.Value == Pascal.sepComma.ToString())
                        {
                        }
                        else if (cnst is Identifier)
                        {
                            caseLabelList.Add(new IdentNode(cnst));
                        }
                        else if (cnst is IntegerLiteral)
                        {
                            caseLabelList.Add(new IntegerNode(cnst));
                        }
                        else if (cnst is StringLiteral)
                        {
                            caseLabelList.Add(new StringNode(cnst));
                        }
                        else throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                                 $"Fatal: Expected constant variable");
                        _lex.NextLexem();
                        cnst = _lex.GetLexem();
                    }
                    Require(Pascal.sepColon.ToString());
                    caseNode.Cases.Add((caseLabelList, ParseStatement()));
                } while (RequireWithoutThrows(Pascal.sepSemicolon.ToString()) &&
                         _lex.GetLexem().Value != Pascal.keyEnd);
                Require(Pascal.keyEnd);
                return caseNode;
            }

            if (_lex.GetLexem().Value == Pascal.sepSemicolon.ToString())
            {
                return new EmptyStatement();
            } 

            throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                $"Fatal: Unexpected lexem {_lex.GetLexem().Value}");
        }

        public Node ParseCallable(Node ident)
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
                    // var lexem = _lex.GetLexem();
                    // if (lexem is Identifier)
                    // {
                    //     _lex.NextLexem();
                    //     if (_lex.GetLexem().Value == Pascal.lexLParent)
                    //         callNode.Args.Add(ParseCallable(new IdentNode(lexem)));
                    //     else 
                    //         callNode.Args.Add(ParseVariable(new IdentNode(lexem)));
                    // }
                    //else
                    {
                        callNode.Args.Add(ParseExpr());
                    }
                }
            }
            
            return callNode;
        }

        public Node ParseArrayAccess(Node ident)
        {
            var arr = new ArrayAccessNode(ident);
            Require(Pascal.sepLBracket.ToString());
            do
            {
                arr.Indexes.Add(ParseExpr());
            } while (RequireWithoutThrows(Pascal.sepComma.ToString()) &&
                     _lex.GetLexem().Value != Pascal.sepRBracket.ToString());
            Require(Pascal.sepRBracket.ToString());
            return arr;
        }

        public Node ParseExpr(Node nd = null)
        {
            Node left = nd ?? ParseTerm();
            AbstractLexem op = _lex.GetLexem();
            while (op.Value == Pascal.opPlus || op.Value == Pascal.opMinus || op.Value == Pascal.keyOr)
            {
                _lex.NextLexem();
                Node right = ParseTerm();
                left = new BinOpNode(left, op, right);
                op = _lex.GetLexem();
            }
            
            AbstractLexem rop = _lex.GetLexem();
            if (Pascal.RelationalOperators.Contains(rop.Value) || rop.Value == Pascal.keyIn)
            {
                _lex.NextLexem();
                return new BinOpNode(left, rop, ParseTerm());
            }

            return left;
        }

        public Node ParseTerm()
        {
            var posUnary = _lex.GetLexem();
            Node left = null;
            if (posUnary.Value == Pascal.opPlus || posUnary.Value == Pascal.opMinus || posUnary.Value == Pascal.opMemoryAdress)
            {
                _lex.NextLexem();
                left = new UnaryOpNode(posUnary, ParseFactor());
            }
            else left = ParseFactor();
            AbstractLexem op = _lex.GetLexem();
            if (op.Value == Pascal.opMult || 
                op.Value == Pascal.opDiv || 
                op.Value == Pascal.opIntDiv || 
                op.Value == Pascal.opMod ||
                op.Value == Pascal.keyAnd)
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
                if (l.Value == Pascal.keyNil)
                    return new NilNode(l);
                if (l.Value == Pascal.keyNot)
                    return new UnaryOpNode(l, ParseFactor()); // Not factor?
                if (_lex.GetLexem().Value == Pascal.lexLParent) // Callable Node
                    return ParseCallable(new IdentNode(l));
                if (_lex.GetLexem().Value == Pascal.sepLBracket.ToString())
                    return ParseArrayAccess(new IdentNode(l));
                return ParseVariable(new IdentNode(l));
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
