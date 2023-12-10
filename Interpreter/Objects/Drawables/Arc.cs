namespace Interpreter;

public class Arc : Figure
{
    private Point p1 ;
    public Point P1 {get => p1;}
    private Point p2 ;
    public Point P2 {get => p2;}
    private Point p3 ;
    public Point P3 {get => p3;}
    private double measure ;
    public double Measure {get => measure;}
    private Color ColorFigure;
    public override Color GetColor => ColorFigure;

    public Arc(Point p1 , Point p2 , Point p3 , double measure)
    {
        this.p1 = p1 ;
        this.p2 = p2 ;
        this.p3 = p3 ;
        this.measure = measure ;
        ColorFigure = Parser.actualColor ;
    }

}