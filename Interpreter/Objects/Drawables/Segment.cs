using System.Data;

namespace Interpreter;

public class Segment : Figure 
{

    public Point P1 { get; }

    public Point P2 { get; }

    public override Color FigureColor { get; set;}

    public override string? Text { get; set; }

    public Equation Equation { get; }

    public Segment(Point p1, Point p2)
    {
        P1 = p1;

        P2 = p2;

        double x1 = P1.X;
        double x2 = P2.X;
        double y1 = P1.Y;
        double y2 = P2.Y;

        Constrains XconstrainsX = new(x1, x2);
        Constrains YconstrainsY = new(y1, y2);

        Equation = Equation.GetLineEquation(P1, P2, XconstrainsX, YconstrainsY);

        FigureColor = Parser.actualColor;
    }

}