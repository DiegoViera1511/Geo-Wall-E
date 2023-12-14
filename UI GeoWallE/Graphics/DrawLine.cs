namespace UI_GeoWallE.Graphics
{
    public class DrawLine : IDrawable
    {
        public Interpreter.Line line;
        public double newY1;
        public double newY2;
        public double newX1;
        public double newX2;

        public DrawLine(Interpreter.Line lin)
        {
            line = lin;

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
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawLine((float)newX1, (float)newY1, (float)newX2, (float)newY2);
        }
    }
}