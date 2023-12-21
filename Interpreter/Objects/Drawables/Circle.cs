namespace Interpreter;

public class Circle : Figure
{
    public Point Center { get; }

    public double Radio { get; }

    public override Color FigureColor { get; set;}

    public override string? Text { get ; set; }

    public Equation Equation { get; }


    public Circle(Point center , double radio)
    {
        Center = center ;
        Radio = radio ;
        Equation = Equation.GetCircleEquation(center, radio);

    }
}