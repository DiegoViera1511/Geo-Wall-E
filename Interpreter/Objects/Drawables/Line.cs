namespace Interpreter;

public class Line : Figure
{
    private Point p1 ;
    public Point P1{get => p1;}
    private Point p2 ;
    public Point P2{get => p2;}

    public Line(Point p1 , Point p2 )
    {
        this.p1 = p1 ;
        this.p2 = p2 ;
    }

}