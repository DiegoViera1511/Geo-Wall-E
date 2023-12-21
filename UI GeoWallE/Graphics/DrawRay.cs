namespace UI_GeoWallE.Graphics
{
    public class DrawRay : IDrawable
    {
        public Interpreter.Ray ray;
        public double Y1;
        public double Y2;
        public double X1;
        public double X2;
        public string color;

        public DrawRay(Interpreter.Ray r)
        {
            ray = r;
            color = r.FigureColor.ToString();

            if (ray.P1.X == ray.P2.X)
            {
                if (ray.P1.Y < ray.P2.Y)
                {
                    X2 = X1 = ray.P1.X;
                    Y1 = ray.P1.Y;
                    Y2 = 10000;
                }

                else
                {
                    X2 = X1 = ray.P1.X;
                    Y1 = ray.P1.Y;
                    Y2 = -1;
                }
            }

            else
            {
                double m = (ray.P2.Y - ray.P1.Y) / (ray.P2.X - ray.P1.X);
                double n = ray.P1.Y - m * ray.P1.X;

                if (ray.P1.X < ray.P2.X)
                {
                    X1 = ray.P1.X;
                    Y1 = ray.P1.Y;
                    X2 = 10000;
                    Y2 = 10000 * m + n;
                }

                else
                {
                    X1 = ray.P1.X;
                    Y1 = ray.P1.Y;
                    X2 = -1;
                    Y2 = n;
                }
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
            canvas.DrawLine((float)X1, (float)Y1, (float)X2, (float)Y2);
            canvas.StrokeColor = Colors.Blue;
            
        }
    }
}
