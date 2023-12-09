namespace Interpreter
{
    public interface IUnaryExpression
    {
        Expression Right {get;}
    }

    public class Negation : Expression, IUnaryExpression
    {
        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Negation(Expression right)
        {
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }
        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Negative : Expression, IUnaryExpression
    {
        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Negative(Expression right)
        {
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }
        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}