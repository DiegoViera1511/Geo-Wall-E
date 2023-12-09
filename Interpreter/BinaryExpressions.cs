using System.Net.NetworkInformation;

namespace Interpreter
{
    public interface IBinaryExpression 
    {
        Expression Left {get;}
        Expression Right {get;}
    }

    public class Addition : Expression ,IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Addition(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Subtraction : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Subtraction(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Multiplication : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Multiplication(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Division : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Division(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Module : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Module(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
    
    public class Power : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Power(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class LogicAND : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public LogicAND(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class LogicOR : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public LogicOR(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Equality : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public Equality(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class NotEquality : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public NotEquality(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ComparisonGREATER : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public ComparisonGREATER(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ComparisonLESS : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public ComparisonLESS(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ComparisonGREATER_EQUAL : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public ComparisonGREATER_EQUAL(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ComparisonLESS_EQUAL : Expression , IBinaryExpression
    {
        private Expression left ;
        public Expression Left {get => left;}

        private Expression right ;
        public Expression Right {get => right;}

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public ComparisonLESS_EQUAL(Expression left , Expression right)
        {
            this.left = left ;
            this.right = right ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }




}