namespace Interpreter;

public class Point : Figure
{      
   
    public double X { get; }

    public double Y{ get; }

    public override Color FigureColor { get; set; }

    public Equation Equation { get; }
    
    public override string? Text { get; set; }

    public Point( double x , double y)
    {
        X = x ;
        Y = y ;
    }
}