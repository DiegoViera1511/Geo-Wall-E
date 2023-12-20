namespace Interpreter;

public class Arc : Figure
{
   
    public Point P1 { get; }
    
    public Point P2 { get; }

    public Point P3 { get; }

    public double Measure { get; }

    public override Color FigureColor { get;}

    public override string? Text { get; set; }

    public Equation Equation { get; }

    private static double GetAngle(Point p1, Point p2) 
    {
        double y = (p2.Y - p1.Y);
        double x = (p2.X - p1.X);
        double angle = Math.Atan2(y, x);
        return angle;
    }

    public Arc(Point p1 , Point p2 , Point p3 , double measure)
    {
        P1 = p1;

        P2 = p2 ;

        P3 = p3 ;

        Measure = measure ;

        double startAngle = GetAngle(P1 , P2);
        double endAngle = GetAngle(P1 , P3);

        Constrains AngleConstrain = new( startAngle, endAngle);
        Equation = Equation.GetCircleEquation(p1, Measure, AngleConstrain);

        FigureColor = Parser.actualColor ;

    }
}

