namespace Interpreter;

public class Segment : Figure 
{

    public Point P1 { get; }

    public Point P2 { get; }

    public override Color FigureColor { get; }

    public override string? Text { get; set; }

    public Segment(Point p1, Point p2)
    {
        P1 = p1;
        P2 = p2;
        FigureColor = Parser.actualColor;
    }

}