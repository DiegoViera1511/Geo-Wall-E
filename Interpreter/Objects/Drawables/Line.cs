namespace Interpreter;

public class Line : Figure
{

    public Point P1{ get; }

    public Point P2{ get; }

    public override Color FigureColor { get; }

    public override string? Text { get ; set; }

    public Equation Equation { get; }

    public Line(Point p1 , Point p2 )
    {
        P1 = p1 ;

        P2 = p2 ;

        Equation = Equation.GetLineEquation(P1, P2);

        FigureColor = Parser.actualColor ;
    }

}