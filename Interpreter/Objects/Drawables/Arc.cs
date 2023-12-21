namespace Interpreter;

public class Arc : Figure
{
   
    public Point P1 { get; }
    
    public Point P2 { get; }

    public Point P3 { get; }

    public double Measure { get; }

    public override Color FigureColor { get; set;}

    public override string? Text { get; set; }

    public Arc(Point p1 , Point p2 , Point p3 , double measure)
    {
        P1 = p1;
        P2 = p2 ;
        P3 = p3 ;
        Measure = measure ;
    }

}