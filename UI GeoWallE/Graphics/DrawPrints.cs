using System;
using System.Collections;
using Interpreter;


namespace UI_GeoWallE.Graphics
{
	public class DrawPrints : IDrawable
	{
        public static List<Interpreter.Circle> kk = new List<Interpreter.Circle> {
            new Interpreter.Circle(new Interpreter.Point(500 , 500) ,25 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,50 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,75 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,100 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,125 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,150 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,175 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,200 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,225 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,250 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,275 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,300 ),
        new Interpreter.Circle(new Interpreter.Point(500 , 500) ,325 )};
        
        

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            for (int i = 0; i < kk.Count; i++)
            {
                if (kk[i] is Interpreter.Circle)
                {
                    DrawCircle printiño = new DrawCircle(kk[i]);

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

