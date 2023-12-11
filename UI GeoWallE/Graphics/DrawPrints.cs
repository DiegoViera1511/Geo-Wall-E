using System;
namespace UI_GeoWallE.Graphics
{
	public class DrawPrints : IDrawable
	{
        
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            for(int i = 0; i < Interpreter.GSharpInterpreter.GraficsViewPrints.Count; i++)
            {
                if (Interpreter.GSharpInterpreter.GraficsViewPrints[i] is Interpreter.Point)
                {
                    Interpreter.Point p = (Interpreter.Point)Interpreter.GSharpInterpreter.GraficsViewPrints[i];
                    DrawPoint dp = new DrawPoint(p);
                    dp.Draw(canvas, dirtyRect);
                }
            }
        }
    }

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

