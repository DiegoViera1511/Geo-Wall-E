namespace UI_GeoWallE.Graphics
{
    public class DrawCircle : IDrawable
    {
        public Interpreter.Circle circle;
        public string color;

        public DrawCircle(Interpreter.Circle cir1)
        {
            color = cir1.FigureColor.ToString();
            circle = cir1;
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
            
            canvas.DrawCircle((float)circle.Center.X, (float)circle.Center.Y, (float)circle.Radio);

        }
    }
}