using System;
using System.Collections;
using Interpreter;
using Microsoft.Maui.Controls.Shapes;


namespace UI_GeoWallE.Graphics
{
	public class DrawPrints : IDrawable
	{
        public static List<Interpreter.Figure> Prints = new List<Interpreter.Figure> {
            new Interpreter.Ray(new Interpreter.Point(452, 563) ,new Interpreter.Point(652 , 263)) };

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            for (int i = 0; i < Prints.Count; i++)
            {
                if (Prints[i] is Interpreter.Circle)
                {
                    DrawCircle toDraw = new DrawCircle((Interpreter.Circle)Prints[i]);

                    toDraw.Draw(canvas, dirtyRect);
                }

                if (Prints[i] is Interpreter.Point)
                {
                    DrawPoint toDraw = new DrawPoint((Interpreter.Point)Prints[i]);

                    toDraw.Draw(canvas, dirtyRect);
                }


                if (Prints[i] is Interpreter.Segment)
                {
                    DrawSegment toDraw = new DrawSegment((Interpreter.Segment)Prints[i]);

                    toDraw.Draw(canvas, dirtyRect);
                }

                if (Prints[i] is Interpreter.Line)
                {
                    DrawLine toDraw = new DrawLine((Interpreter.Line)Prints[i]);

                    toDraw.Draw(canvas, dirtyRect);
                }

                if (Prints[i] is Interpreter.Ray)
                {
                    DrawRay toDraw = new DrawRay((Interpreter.Ray)Prints[i]);

                    toDraw.Draw(canvas, dirtyRect);
                }

                if (Prints[i] is Interpreter.Arc)
                {
                   // DrawArc toDraw = new DrawArc((Interpreter.Arc)Prints[i]);

                   // toDraw.Draw(canvas, dirtyRect);
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
        public Interpreter.Line line;
        public double newY1;
        public double newY2;
        public double newX1;
        public double newX2;

        public DrawLine(Interpreter.Line lin)
        {
            line = lin;

            if (line.P1.X == line.P2.X)
            {
                newX1 = newX2 = line.P1.X;
                newY1 = 0;
                newY2 = 10000;
            }

            else
            {
                double m = (line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
                double n = line.P1.Y - m * line.P1.X;

                 newX1 = 0;
                 newY1 = n;
                 newX2 = 10000;
                 newY2 = 10000 * m + n;
            }

        }
            
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawLine((float)newX1, (float)newY1, (float)newX2, (float)newY2);
        }
    }

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
            canvas.DrawLine((float)segment.P1.X , (float)segment.P1.Y , (float)segment.P2.X , (float)segment.P2.Y);
        }
    }

    public class DrawRay : IDrawable
    {
        public Interpreter.Ray ray;
        public double Y1;
        public double Y2;
        public double X1;
        public double X2;

        public DrawRay(Interpreter.Ray r)
        {
            ray = r;

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

                if(ray.P1.X < ray.P2.X)
                {
                    X1 = ray.P1.X;
                    Y1 = ray.P1.Y;
                    X2 = 10000;
                    Y2 = 10000 * m +n;
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
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Black;
            canvas.DrawLine((float)X1, (float)Y1, (float)X2, (float)Y2);
            canvas.StrokeColor = Colors.Blue;
            canvas.DrawCircle(452, 563, 3);
            canvas.DrawCircle(652, 263, 3);
        }
    }

    public class DrawArc : IDrawable
    {
        public Interpreter.Arc arc;
        public double radius;
        public Interpreter.Point center;
        public double startAngle;
        public double endAngle;

        static double GetAngle(Interpreter.Point p1, Interpreter.Point p2) //siempre entrar como p1 el centro
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
            startAngle = (int)(360 - GetAngle(arc.P1, arc.P3) * (180 / Math.PI));
            endAngle = (int)(360 - GetAngle(arc.P1, arc.P2) * (180 / Math.PI));

        }


       


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