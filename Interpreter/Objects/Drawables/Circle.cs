namespace Interpreter;

public class Circle : Figure
{
    private Point center ;
    public Point Center {get => center;}
    private double radio ;
    public double Radio {get => radio;}
    private Color ColorFigure;
    public override Color GetColor => ColorFigure;
    public Circle(Point center , double radio)
    {
        this.center = center ;
        this.radio = radio ;
        ColorFigure = Parser.actualColor ;
    }
}