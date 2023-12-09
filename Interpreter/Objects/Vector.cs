using System;
namespace Interpreter.Objects
{
	public class Vector
	{

		public double X { get; }
		public double Y { get; }
		public double Rule { get; } //norma  del vector


		public Vector(double x, double y)
		{
			X = x;
			Y = y;
			Rule = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
		}

		public Vector VectorRuled() //normalizar un vector
		{
			double x = X / Rule;
			double y = Y / Rule;

			return new Vector(x, y);
		}

		public Vector MultiplyByScalar (double scalar , Vector vec) //multiplicación por un escalar
			{

            double x = vec.X * scalar;
            double y = vec.Y * scalar;

			return new Vector(x, y);
        }
	}
}

