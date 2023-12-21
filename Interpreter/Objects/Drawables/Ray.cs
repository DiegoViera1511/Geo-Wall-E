namespace Interpreter;

public class Ray : Figure
{

    public Point P1{ get; }

    public Point P2{ get; }

    public override Color FigureColor { get; set; }

    public override string? Text { get ; set ; }

    public Ray(Point p1 , Point p2)
    {
        P1 = p1 ;
        P2 = p2 ;
    }
        
}