using System.Collections;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Interpreter
{

    public enum TypeSequence
    {
        Infinity , Range , Values 
    }
    public abstract class Sequence : Expression 
    {
        public abstract TypeSequence GetSequenceType{get ;}
    }
    
    public class ValuesSecquence : Sequence
    {
        private List<Expression> secquenceValues = new List<Expression>();
        public List<Expression> SequenceValues {get => secquenceValues;}
        public TypeSequence SequenceType = TypeSequence.Values ;
        public override TypeSequence GetSequenceType => SequenceType ;
        private int expressionLine ;
        public override int ExpressionLine => expressionLine;

        public ValuesSecquence(List<Expression> secquenceValues)
        {
            this.secquenceValues = secquenceValues ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }

    public class InfinitySequence : Sequence , IEnumerable<object>
    {

        private double firstNumber ;
        public double FirstNumber{get => firstNumber;} 

        public TypeSequence SequenceType = TypeSequence.Infinity ;
        public override TypeSequence GetSequenceType => SequenceType ;

        private int expressionLine ;
        public override int ExpressionLine => expressionLine;
        
        public InfinitySequence(double firstNumber)
        {
            this.firstNumber = firstNumber ;
            expressionLine = Parser.GetLine ;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public IEnumerator<object> GetEnumerator()
        {
            double n = firstNumber ;
            while(true)
            {
                yield return n ;
                n++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            double n = firstNumber ;
            while(true)
            {
                yield return n ;
                n++;
            }
        }
    }

    public class RangeSequence : Sequence , IEnumerable<object>
    {
        private double inf ;
        public double Inf {get => inf;}

        private double max ;
        public double Max {get => max;}
        
        public TypeSequence SequenceType = TypeSequence.Range ;
        public override TypeSequence GetSequenceType => SequenceType ;

        private int expressionLine ;
        public override int ExpressionLine => expressionLine ;

        public RangeSequence(double inf , double max)
        {
            this.inf = inf ;
            this.max = max ;
            expressionLine = Parser.GetLine ;
        }

        public IEnumerator<object> GetEnumerator()
        {
            for(double i = inf ; i <= max ; i++)
            {
                yield return i ;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for(double i = inf ; i <= max ; i++)
            {
                yield return i ;
            }
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
    
   
    
    

}