using System.Reflection;

namespace Interpreter
{
    public abstract class StatementExpression : IExpression
    {
        private int expressionLine ;
        public int ExpressionLine {get => expressionLine;}

        public abstract object? Accept(IVisitor visitor);
        //Los statements no devuelven valor 
        public class FunctionDeclaration : StatementExpression
        {
            public static List<string> DeclaredFunctions = new();
            private List<string> parameters = new();
            public List<string> Parameters {get => parameters;}
            private string functionName ;
            public string FunctionName {get => functionName;}
            private Expression body ;
            public Expression Body{get => body;}
            private Context functionContext ;
            public Context FunctionContext {get => functionContext;}
            
            public FunctionDeclaration(string functionName , List<string> parameters , Expression body , Context functionContext)
            {
                this.functionName = functionName ;
                this.parameters = parameters;
                this.body = body ;
                this.functionContext = functionContext ;
                expressionLine = Parser.GetLine ;
            }

            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);   
            }
        }

        public class VariableDeclaration : StatementExpression 
        {
            public static List<string> DeclaredVariables = new();
            private string variableName ;
            private Expression value ;
            public string VariableName{get => variableName;}
            public Expression Value{get => value;}
           
            public VariableDeclaration (string variableName , Expression value)
            {
                this.variableName = variableName;
                this.value = value ;
                expressionLine = Parser.GetLine ;
            }
           
            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class VariableSequenceDeclaration : StatementExpression
        {
            private List<string> variablesNames = new();
            public List<string> VariablesNames {get => variablesNames;}
            private Sequence valuesSequence ;
            public Sequence ValuesSequence {get => valuesSequence;}
            

            public VariableSequenceDeclaration(List<string> variablesNames , Sequence valuesSequence)
            {
                this.variablesNames = variablesNames ;
                this.valuesSequence = valuesSequence ;
                expressionLine = Parser.GetLine ;
            }

            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class RandomPoint : StatementExpression
        {
            
            public static List<string> DeclaredPoints = new();
            private string name ;
            public string Name {get => name;}
            private double x ;
            public double X{get => x;}
            private double y ;
            public double Y {get => y;}
            
            public RandomPoint( double x , double y , string name)
            {
                this.x = x ;
                this.y = y ;
                this.name = name ;
                expressionLine = Parser.GetLine ;
            }

            public static RandomPoint NewPoint(string name)
            {
                Random number = new Random();
                return new RandomPoint(number.Next(100 , 700) , number.Next(100 , 700) , name);
            }
            
            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }

        }
        
        public class RandomLine : StatementExpression
        {
            private string name ;
            public string Name {get => name;}
            private string lineType ;
            public string LineType{get => lineType;}

            public RandomLine(string name , string lineType)
            {
                this.name = name ;
                this.lineType = lineType ;
                expressionLine = Parser.GetLine ;
            }

            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }
        
        public class RandomCircle : StatementExpression
        {
            private string name ;
            public string Name {get => name;}
           
            public RandomCircle(string name)
            {
                this.name = name ;
                expressionLine = Parser.GetLine ;
            }

            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class SetColor : StatementExpression
        {
            private Color color ;
            public Color Color {get => color;}

            public SetColor(Color color)
            {
                this.color = color ;
                expressionLine = Parser.GetLine ;
            }

            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }
        
        public class Restore : StatementExpression
        {
            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Draw : StatementExpression
        {
            private Expression figureDraw;
            public Expression FigureDraw{get => figureDraw;}
            public Draw (Expression figureDraw)
            {
                this.figureDraw = figureDraw ;
                expressionLine = Parser.GetLine ;
            }

            public override object? Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Import : StatementExpression
        {
            private string fileName ;
            public string FileName{get => fileName;}

            public Import(string fileName)
            {
                this.fileName = fileName ;
                expressionLine = Parser.GetLine ;
            }

            public override object? Accept(IVisitor visitor)
            {
                throw new NotImplementedException();
            }
        }

    }
}