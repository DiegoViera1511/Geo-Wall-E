using System;
using System.Numerics;
namespace Interpreter
{
	public class Measure
	{
		
		public double Value; // el valor de la medida
		public Measure(Point p1 , Point p2)
		{
			double x1 = p1.X;
			double y1 = p1.Y;
			double x2 = p2.X;
			double y2 = p2.Y;
			Value = Math.Sqrt(Math.Pow(x2-x1, 2) + Math.Pow(y2-y1,2)); //distancia de punto a punto
		}
		
		public Measure(double value)
		{
			Value = value;
		}
		
		
	}
}

