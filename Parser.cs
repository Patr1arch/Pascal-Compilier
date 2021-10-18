using System;
using System.Collections.Generic;
using myPascal.Lexems;
using myPascal.Nodes;

namespace myPascal
{
    public class Parser
    {
        private Lexer _lex;

        public Parser(Lexer lex = null)
        {
            _lex = lex;
            _lex?.NextLexem();
        }

        public void SetParser(Lexer lex)
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
        public bool Ask(string lexemName)
        {
            if (_lex.GetLexem().Value.ToLower() == lexemName)
            {
                _lex.NextLexem();
                return true;
            }

            return false;
        }

        public bool Ask(List<string> lst)
        {
            foreach (var str in lst)
            {
                if (Ask(str))
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
                if (_lex.GetLexem().Value.ToLower() == "end." || _lex.GetLexem().Value.ToLower() == Pascal.keyEnd || 
                    _lex.GetLexem().Value.ToLower() == Pascal.keyUntil)
                    break;
                seq.Statements.Add(ParseStatement());
            } while (Ask(Pascal.sepSemicolon.ToString()));
            return seq;
        }
        
        public Node ParseVariable(Node nd = null)
        {
            Node ident = null;
            
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
                specialNode = ParseVariable(ParseArrayAccess(ident));
            }
            else if (_lex.GetLexem().Value == Pascal.lexLParent)
                specialNode = ParseVariable(ParseCallable(ident));

            if (Ask(Pascal.opCaret))
            {
                if (Ask(Pascal.sepDot.ToString()))
                {
                    if (_lex.GetLexem() is Identifier)
                        return ParseVariable(new RecordFieldAccessNode(new PointerNode(ident), ParseVariable()));
                    else throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                             $"Fatal: Expected record field identifier");
                }
                return ParseVariable(new PointerNode(specialNode ?? ident));
            }
            
            if (Ask(Pascal.sepDot.ToString()))
            {
                if (_lex.GetLexem() is Identifier)
                    return ParseVariable(new RecordFieldAccessNode(ident, ParseVariable()));
                else throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                         $"Fatal: Expected record field identifier");
            }

            return specialNode ?? ident;
        }

        public Node ParseStatement()
        {
            if (_lex.GetLexem() is Identifier)
            {
                Node variable = ParseVariable();
                if (Ask(Pascal.opAssign)) // It's assign statement
                {
                    return new AssignStatement(variable, ParseExpr());
                }
                else
                {
                    return ParseCallable(variable);
                }
            }

            if (Ask(Pascal.keyBegin))
            {
                var stmt = ParseStatementSequence();
                Require(Pascal.keyEnd);
                return stmt;
            }

            if (Ask(Pascal.keyWhile))
            {
                var whileNode = new WhileNode(ParseExpr());
                Require(Pascal.keyDo);
                whileNode.Body = ParseStatement();
                return whileNode;
            }

            if (Ask(Pascal.keyRepeat))
            {
                var repeatNode = new RepeatNode(ParseStatementSequence()); // ParseStatementSequence() ?
                Require(Pascal.keyUntil);
                repeatNode.UntilCondition = ParseExpr();
                return repeatNode;
            }

            if (Ask(Pascal.keyFor))
            {
                if (_lex.GetLexem() is Identifier)
                {
                    var forNode = new ForNode(_lex.GetLexem());
                    _lex.NextLexem();
                    Require(Pascal.opAssign);
                    forNode.InitialExpr = ParseExpr();
                    if (Ask(Pascal.keyTo))
                        forNode.IsTo = true;
                    else if (Ask(Pascal.keyDownto))
                        forNode.IsTo = false;
                    else throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                                   $"Fatal: Syntax error, \"{Pascal.lexRParent}\" expected but " +
                                                   $"{_lex.GetLexem().SourceCode} found");
                    forNode.FinalExpr = ParseExpr();
                    Require(Pascal.keyDo);
                    forNode.Body = ParseStatement();
                    return forNode;
                }
                else
                    throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                    $"Fatal: Expected variable identifier");
            }

            if (Ask(Pascal.keyIf))
            {
                var ifNode = new IfNode(ParseExpr());
                Require(Pascal.keyThen);
                ifNode.ThenStmt = ParseStatement();
                if (Ask(Pascal.keyElse))
                    ifNode.ElseStmt = ParseStatement();
                return ifNode;
            }

            if (Ask(Pascal.keyCase))
            {
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
                } while (Ask(Pascal.sepSemicolon.ToString()) &&
                         _lex.GetLexem().Value != Pascal.keyEnd);
                Require(Pascal.keyEnd);
                return caseNode;
            }

            if (Ask(Pascal.sepSemicolon.ToString()))
            {
                return new EmptyStatement();
            } 

            throw new Exception($"{_lex.FilePath}{_lex.GetLexem().Coordinates} " +
                                $"Fatal: Unexpected lexem {_lex.GetLexem().Value}");
        }

        public Node ParseCallable(Node ident)
        {
            var callNode = new CallNode(ident);
            if (Ask(Pascal.lexLParent))
            {
                int count = 0;
                while (!Ask(Pascal.lexRParent) && !_lex.IsEOFReached)
                {
                    if (count++ != 0)
                        Require(Pascal.sepComma.ToString());
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
            } while (Ask(Pascal.sepComma.ToString()) &&
                     _lex.GetLexem().Value != Pascal.sepRBracket.ToString());
            Require(Pascal.sepRBracket.ToString());
            return arr;
        }

        public Node ParseExpr()
        {
            Node left = ParseSimpleExpr();
            AbstractLexem rop = _lex.GetLexem();
            if (Ask(Pascal.RelationalOperators) || Ask(Pascal.keyIn))
            {
                return new BinOpNode(left, rop, ParseSimpleExpr());
            }

            return left;
        }
        
        public Node ParseSimpleExpr()
        {
            Node left = ParseTerm();
            AbstractLexem op = _lex.GetLexem();
            while (Ask(Pascal.opPlus) || Ask(Pascal.opMinus) || Ask(Pascal.keyOr))
            {
                Node right = ParseTerm(); 
                left = new BinOpNode(left, op, right);
                op = _lex.GetLexem();
            }

            return left;
        }

        public Node ParseTerm()
        {
            var left = ParseFactor();
            AbstractLexem op = _lex.GetLexem();
            while (Ask(Pascal.opMult) || 
                Ask(Pascal.opDiv) || 
                Ask(Pascal.opIntDiv) || 
                Ask(Pascal.opMod) ||
                Ask(Pascal.keyAnd))
            {
                Node right = ParseFactor();
                left = new BinOpNode(left, op, right);
                op = _lex.GetLexem();
            }

            return left;
        }

        public Node ParseFactor()
        {
            var l = _lex.GetLexem();
            _lex.NextLexem();
            if (l is Identifier || l.Value == Pascal.keyTrue || l.Value == Pascal.keyFalse)
            {
                if (_lex.GetLexem().Value == Pascal.lexLParent) // Callable Node
                    return ParseCallable(new IdentNode(l));
                if (_lex.GetLexem().Value == Pascal.sepLBracket.ToString())
                    return ParseArrayAccess(new IdentNode(l));
                return ParseVariable(new IdentNode(l));
            }
            
            if (l.Value == Pascal.keyNil)
                return new NilNode(l);
            if (l.Value == Pascal.keyNot ||
                l.Value == Pascal.opPlus ||
                l.Value == Pascal.opMinus || 
                l.Value == Pascal.opMemoryAdress)
                return new UnaryOpNode(l, ParseFactor());

            if (l is IntegerLiteral)
            {
                return new IntegerNode(l);
            }

            if (l is StringLiteral)
            {
                return new StringNode(l);
            }

            if (l is RealLiteral)
            {
                return new RealNode(l);
            }

            if (l.Value == Pascal.lexLParent)
            {
                Node e = ParseExpr();
                Require(Pascal.lexRParent);
                return e;
            }
    
            throw new Exception($"{_lex.FilePath}{l.Coordinates} Fatal: Unexpected lexem {l.Value}");
        }
    }
}
