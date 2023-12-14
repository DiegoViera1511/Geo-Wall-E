namespace UI_GeoWallE.Graphics
{
    public class DrawArc : IDrawable
    {
        public Interpreter.Arc arc;
        public double radius;
        public Interpreter.Point center;
        public double startAngle;
        public double endAngle;

        private static double GetAngle(Interpreter.Point p1, Interpreter.Point p2) //siempre entrar como p1 el centro
        {
            double y = (p2.Y - p1.Y);
            double x = (p2.X - p1.X);
            double angle = Math.Atan2(y, x);
            return angle;
        }


        public DrawArc(Interpreter.Arc a)
        {
            arc = a;
            radius = arc.Measure;
            center = new Interpreter.Point(arc.P1.X - radius, arc.P1.Y - radius);
            startAngle = (int)(360 - GetAngle(arc.P1, arc.P2) * (180 / Math.PI));
            endAngle = (int)(360 - GetAngle(arc.P1, arc.P3) * (180 / Math.PI));

        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawArc((float)center.X , (float)center.Y , (float)(2 * radius) ,
                (float)(2 * radius) , (float)startAngle , (float)endAngle, false , false);
        }
    }
}