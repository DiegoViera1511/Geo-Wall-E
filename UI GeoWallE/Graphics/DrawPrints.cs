using System;
using System.Collections;
using Interpreter;


namespace UI_GeoWallE.Graphics
{
	public class DrawPrints : IDrawable
	{
        public static List<Interpreter.Point> kk = new List<Interpreter.Point> { new Interpreter.Point(200 ,460)};
        
        static Interpreter.Point p1 = new Interpreter.Point(100, 500);
        static Interpreter.Point p2 = new Interpreter.Point(200, 400);
        static Interpreter.Point p3 = new Interpreter.Point(300, 300);
        static Interpreter.Point p4 = new Interpreter.Point(400, 200);
        static Interpreter.Point p5 = new Interpreter.Point(500, 100);

        public static DrawPoint sample1 = new DrawPoint(p1);
        public static DrawPoint sample2 = new DrawPoint(p2);
        public static DrawPoint sample3 = new DrawPoint(p3);
        public static DrawPoint sample4 = new DrawPoint(p4);
        public static DrawPoint sample5 = new DrawPoint(p5);

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            for (int i = 0; i < kk.Count; i++)
            {
                if (kk[i] is Interpreter.Point)
                {
                    DrawPoint printiño = new DrawPoint(kk[i]);

                    printiño.Draw(canvas, dirtyRect);
                   
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
            canvas.DrawCircle( (float)point.X , (float)point.Y , 2 );
        }
    }

    public class DrawLine : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            
        }
    }

    public class DrawSegment : IDrawable
    {
        public Interpreter.Segment segment;

        public DrawSegment(Interpreter.Segment seg1)
        {
            segment = seg1;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawLine((float)segment.P1.X , (float)segment.P1.Y , (float)segment.P2.X , (float)segment.P2.Y);
        }
    }

    public class DrawRay : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            
        }
    }

    public class DrawArc : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

        }
    }

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

