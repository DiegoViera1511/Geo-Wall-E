namespace UI_GeoWallE.Graphics
{
    public class DrawSegment : IDrawable
    {
        public Interpreter.Segment segment;

        public DrawSegment(Interpreter.Segment seg)
        {
            segment = seg;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawLine((float)segment.P1.X, (float)segment.P1.Y, (float)segment.P2.X, (float)segment.P2.Y);
        }
    }
}