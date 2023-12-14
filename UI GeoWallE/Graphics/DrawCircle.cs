namespace UI_GeoWallE.Graphics
{
    public class DrawCircle : IDrawable
    {
        public Interpreter.Circle circle;

        public DrawCircle(Interpreter.Circle cir1)
        {
            circle = cir1;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawCircle((float)circle.Center.X, (float)circle.Center.Y, (float)circle.Radio);

        }
    }
}