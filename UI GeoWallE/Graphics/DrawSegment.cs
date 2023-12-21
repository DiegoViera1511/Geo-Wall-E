namespace UI_GeoWallE.Graphics
{
    public class DrawSegment : IDrawable
    {
        public Interpreter.Segment segment;
        public string color;
        public DrawSegment(Interpreter.Segment seg)
        {
            segment = seg;
            color = seg.FigureColor.ToString();
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
       
            canvas.DrawLine((float)segment.P1.X, (float)segment.P1.Y, (float)segment.P2.X, (float)segment.P2.Y);
        }
    }
}