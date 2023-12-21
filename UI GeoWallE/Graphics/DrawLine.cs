namespace UI_GeoWallE.Graphics
{
    public class DrawLine : IDrawable
    {
        public Interpreter.Line line;
        public double newY1;
        public double newY2;
        public double newX1;
        public double newX2;
        public string color;

        public DrawLine(Interpreter.Line lin)
        {
            line = lin;
            color = lin.FigureColor.ToString();

            if (line.P1.X == line.P2.X)
            {
                newX1 = newX2 = line.P1.X;
                newY1 = 0;
                newY2 = 10000;
            }

            else
            {
                double m = (line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
                double n = line.P1.Y - m * line.P1.X;

                newX1 = 0;
                newY1 = n;
                newX2 = 10000;
                newY2 = 10000 * m + n;
            }

        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

            if (color == "Black") canvas.StrokeColor = Colors.Black;
            else if (color == "White") canvas.StrokeColor = Colors.White;
            else if (color == "Green") canvas.StrokeColor = Colors.Green;
            else if (color == "Gray") canvas.StrokeColor = Colors.Gray;
            else if (color == "Magenta") canvas.StrokeColor = Colors.Magenta;
            else if (color == "Cyan") canvas.StrokeColor = Colors.Cyan;
            else if (color == "Blue") canvas.StrokeColor = Colors.Blue;
            else if (color == "Red") canvas.StrokeColor = Colors.Red;
            else if (color == "Yellow") canvas.StrokeColor = Colors.Yellow;

            canvas.StrokeSize = 4;
            
            canvas.DrawLine((float)newX1, (float)newY1, (float)newX2, (float)newY2);
        }
    }
}