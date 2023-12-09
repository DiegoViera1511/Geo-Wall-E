using System.IO.Compression;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Interpreter
{
    public interface IExpression 
    {
        object? Accept(IVisitor visitor);
        int ExpressionLine{get ;}

    }
    public abstract class Expression : IExpression
    {
        public abstract object Accept(IVisitor visitor) ;
        
        public abstract int ExpressionLine{get;}

        public abstract class Atom : Expression
        {
            public abstract object Value{get ;}
            public abstract ExpressionType ExpType{get;}



            public class Number : Atom
            {
                public object value ;
                public override object Value{get => value ;}
                private ExpressionType expType = ExpressionType.Number ;
                public override ExpressionType ExpType {get => expType ;}

                private int expressionLine ;
                public override int ExpressionLine => expressionLine;

                public Number(double value)
                {
                    this.value = value ;
                    expressionLine = Parser.GetLine;
                }
            }
            public class String : Atom
            {
                public object value ;
                public override object Value{get => value;}
                private ExpressionType expType = ExpressionType.String ;
                public override ExpressionType ExpType {get => expType ;}

                private int expressionLine ;
                public override int ExpressionLine => expressionLine;

                public String (string value)
                {
                    this.value = value ;
                    expressionLine = Parser.GetLine ;
                }
            }
            public class Boolean : Atom 
            {
                public object value ;
                public override object Value{get => value ;}
                private ExpressionType expType = ExpressionType.Boolean ;
                public override ExpressionType ExpType {get => expType ;}

                private int expressionLine ;
                public override int ExpressionLine => expressionLine;

                public Boolean(bool value)
                {
                    this.value = value ;
                    expressionLine = Parser.GetLine ;
                }
            }

            public class Undefined : Expression
            {
                private ExpressionType expType = ExpressionType.Undefined ;
                public ExpressionType ExpType{get => expType;}

                private int expressionLine ;
                public override int ExpressionLine => expressionLine;
                
                public Undefined()
                {
                    expressionLine = Parser.GetLine ;
                }

                public override object Accept(IVisitor visitor)
                {
                    return visitor.Visit(this);
                }
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
  
        }   

        public class ID : Expression 
        {
            public string name ;
            public bool IsFunctionName ;
            public string Name{get => name ;}
            private ExpressionType expType = ExpressionType.Inference ;
            public ExpressionType ExpType {get => expType ;}

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;
            
            public ID(string name , bool IsFunctionName)
            {
                this.name = name ;
                this.IsFunctionName = IsFunctionName ;
                expressionLine = Parser.GetLine ;
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Let_In : Expression
        {
            public static Context LetInVariables = new();
            public List<StatementExpression> LetInStatements = new();
            public Dictionary<string , IExpression> LocalVariables = new();
            public Expression Body ;

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;
            public Let_In(Expression Body , Dictionary<string , IExpression> LocalVariables , List<StatementExpression> LetInStatements )
            {
                this.Body = Body ;
                this.LocalVariables = LocalVariables ;
                this.LetInStatements = LetInStatements ;
                expressionLine = Parser.GetLine ;
            }

            public static void RemoveLocalVariables(Dictionary<string , IExpression> localVariables)
            {
                foreach(string x in localVariables.Keys)
                {
                    if(Parser.CurrentContext.Variables.ContainsKey(x))
                    {
                        Parser.CurrentContext.Variables.Remove(x);
                    }
                    else if(Parser.CurrentContext.FiguresVariables.ContainsKey(x))
                    {
                        Parser.CurrentContext.FiguresVariables.Remove(x);
                    } 
                }
            }
            public static void AddLocalVariables(Dictionary<string , IExpression> localVariables)
            {
                foreach(string x in localVariables.Keys)
                {
                    if(Parser.CurrentContext.Variables.ContainsKey(x) || Parser.CurrentContext.FiguresVariables.ContainsKey(x))
                    {
                        throw new Exception ("Duplicated variable name");
                    }
                    if(localVariables[x] is Expression)
                    {
                        Parser.CurrentContext.Variables.Add(x , (Expression)localVariables[x]);
                    }
                    else if(localVariables[x] is Figure)
                    {
                        Parser.CurrentContext.FiguresVariables.Add(x , (Figure)localVariables[x]);
                    }
                }
            }

            public static StatementExpression NewLetInStatements()
            {
                if(Parser.MatchToken(TokenType.ID))
                {
                    Parser.Next();
                    if(Parser.MatchToken(TokenType.EQUAL))
                    {
                        return Parser.VariableDeclarationStmts();
                    }
                    else if(Parser.MatchToken(TokenType.COMMA))
                    {
                        return Parser.VariableSequenceDeclarationStmts();
                    }
                    else if(Parser.MatchToken(TokenType.LEFT_PARENTHESIS))
                    {
                        return Parser.FunctionDeclarationStmst();
                    }
                    else throw new UnexpectedToken(Parser.CurrentToken.StringForm);
                }
                else if(Parser.MatchToken(TokenType.COLOR))
                {
                    Parser.Next();
                    return Parser.ColorStmts();
                }
                else if(Parser.MatchToken(TokenType.DRAW))
                {
                    Parser.Next();
                    return Parser.DrawStmts();
                }
                else if(Parser.MatchToken(TokenType.RESTORE))
                {
                    Parser.Next();
                    return new StatementExpression.Restore();
                }
                else if(Parser.MatchToken(TokenType.POINT))
                {
                    Parser.Next();
                    return Parser.RandomPointStmts();
                }
                else if(Parser.MatchToken(TokenType.CIRCLE))
                {
                    Parser.Next();
                    return Parser.RandomCircleStmts() ;
                }    
                else if(Parser.MatchToken(TokenType.LINE) ||Parser.MatchToken(TokenType.SEGMENT) || Parser.MatchToken(TokenType.RAY))
                {
                    return Parser.RandomLineStmts();
                }
                else if(Parser.MatchToken(TokenType.SKIP))
                {
                    Parser.Next();
                    return Parser.VariableSequenceDeclarationStmts();
                }
                else if(Parser.MatchToken(TokenType.IMPORT))
                {
                    Parser.Next();
                    return Parser.ImportStmts();
                }
                else
                {
                    throw new DefaultError("! Error: The expression is not a valid statement");
                }
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Conditional : Expression
        {
            public Expression IfExpression ;
            public Expression ThenExpression ;
            public Expression ElseExpression ;

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;

            public Conditional(Expression IfExpression , Expression ThenExpression , Expression ElseExpression)
            {
                this.IfExpression = IfExpression ;
                this.ThenExpression = ThenExpression ;
                this.ElseExpression = ElseExpression ;
                expressionLine = Parser.GetLine ;
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class FunctionCall : Expression
        {
            public static Dictionary<string , double> FunctionCallStack = new();
            public string FunctionName ;
            public List<Expression> ParametersValue ;

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;

            public FunctionCall(string FunctionName , List<Expression> ParametersValue )
            {
                this.FunctionName = FunctionName ;
                this.ParametersValue = ParametersValue ;
                expressionLine = Parser.GetLine ;
            }
            
            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public abstract class FigureDeclaration : Expression {}
        
       
        public class PointDeclaration : FigureDeclaration
        {

            private Expression x ;
            public Expression X{get => x;}
            private Expression y ;
            public Expression Y{get => y;}

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;
            public PointDeclaration( Expression x , Expression y)
            {
                this.x = x ;
                this.y = y ;
                expressionLine = Parser.GetLine ;
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class LineDeclaration : FigureDeclaration
        {
            private string lineType ;
            public string LineType{get => lineType;}
            private string p1Name ;
            public string P1Name{get => p1Name;}
            private string p2Name ;
            public string P2Name{get => p2Name;}

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;
            
            public LineDeclaration(string p1Name , string p2Name , string lineType)
            {
                this.p1Name = p1Name ;
                this.p2Name = p2Name ;
                this.lineType = lineType ;
                expressionLine = Parser.GetLine ;
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }

        }

        public class ArcDeclaration :  FigureDeclaration
        {
            private string p1Name ;
            public string P1Name {get => p1Name;}
            private string p2Name ;
            public string P2Name {get => p2Name;}
            private string p3Name ;
            public string P3Name {get => p3Name;}
            private Expression measure ;
            public Expression Measure {get => measure;}

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;
            public ArcDeclaration(string p1 , string p2 , string p3 , Expression measure)
            {
                this.p1Name = p1 ;
                this.p2Name = p2 ;
                this.p3Name = p3 ;
                this.measure = measure ; 
                expressionLine = Parser.GetLine ;
            }
            
            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class CircleDeclaration : FigureDeclaration
        {
            private string centerName ;
            public string CenterName{get => centerName;}
            private Expression measure ;
            public Expression Measure{get => measure;}

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;

            public CircleDeclaration(string centerName , Expression measure)
            {
                this.centerName = centerName ;
                this.measure = measure ;
                expressionLine = Parser.GetLine ;
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class MeasureExpression : Expression
        {
            private string p1Name ;
            public string P1Name {get => p1Name;}
            private string p2Name ;
            public string P2Name {get => p2Name;}

            private int expressionLine ;
            public override int ExpressionLine => expressionLine;

            public MeasureExpression(string p1Name , string p2Name)
            {
                this.p1Name = p1Name ;
                this.p2Name = p2Name ;
                expressionLine = Parser.GetLine ;
            }

            public override object Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }
        
    }

    public class Function 
    {
        public static Dictionary<string , Function> FunctionStore = new();
        
        public string FunctionName ;
        public Expression Body ;
        public List<string> Parameters ;

        public Function(string FunctionName , Expression Body , List<string> Parameters)
        {
            this.FunctionName = FunctionName ;
            this.Body = Body ;
            this.Parameters = Parameters ;
        }
    }

}