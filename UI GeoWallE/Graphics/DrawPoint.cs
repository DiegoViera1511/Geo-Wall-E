namespace UI_GeoWallE.Graphics
{
    public class DrawPoint : IDrawable
    {
        public Interpreter.Point point;


        public DrawPoint(Interpreter.Point p1)
        {
            point = p1;
        }


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawCircle((float)point.X, (float)point.Y, 2);
        }
    }
}