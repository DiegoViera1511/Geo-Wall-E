namespace Interpreter;

public class Point : Figure
{      
    private double x ;
    public double X{get => x;}
    private double y ;
    public double Y{get => y;}
    private Color ColorFigure;
    public override Color GetColor => ColorFigure;
    public Point( double x , double y)
    {
        this.x = x ;
        this.y = y ;
        ColorFigure = Parser.actualColor ;
    }
}