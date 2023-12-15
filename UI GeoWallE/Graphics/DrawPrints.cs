using System;
using System.Collections;
using Interpreter;
using Microsoft.Maui.Controls.Shapes;


namespace UI_GeoWallE.Graphics
{
    public class DrawPrints : IDrawable
    {
        public static List<Interpreter.Figure> Prints = new List<Interpreter.Figure>();
          
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
                     DrawArc toDraw = new DrawArc((Interpreter.Arc)Prints[i]);

                     toDraw.Draw(canvas, dirtyRect);
                }
            }
        }
    }
}