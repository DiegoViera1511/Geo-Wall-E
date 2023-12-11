using System;
namespace Interpreter
{
	public interface IDraw
	{
        public void DrawPoint(Point point);
        public void DrawLine(Line line);
        public void DrawSegment(Segment segment);
        public void DrawCircle(Circle circle);
        public void DrawArc(Arc arc);
        public void DrawRay(Ray ray);
    }
}

