namespace Interpreter;

public class Ray : Figure
{

    public Point P1{ get; }

    public Point P2{ get; }

    public override Color FigureColor { get; set; }

    public override string? Text { get ; set ; }

    public Equation Equation { get; }

    public Ray(Point p1 , Point p2)
    {
        P1 = p1 ;

        P2 = p2 ;

        double x1 = P1.X;
        double x2 = P2.X;
        double y1 = P1.Y;
        double y2 = P2.Y;
        double XlimitX;
        double YlimitY;

        if (x1 == x2) XlimitX = x1;
        else if (x1 < x2) XlimitX = double.PositiveInfinity;
        else XlimitX = double.NegativeInfinity;

        if (y1 == y2) YlimitY = y1;
        else if (y1 < y2) YlimitY = double.PositiveInfinity;
        else YlimitY = double.NegativeInfinity;

        Constrains XconstrainsX = new( x1, XlimitX);
        Constrains YconstrainsY = new( y1, YlimitY);

        Equation = Equation.GetLineEquation(P1, P2, XconstrainsX, YconstrainsY);

        FigureColor = Parser.actualColor ;

    }

}



        