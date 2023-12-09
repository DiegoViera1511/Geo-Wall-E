using System;
namespace Interpreter
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
		
/// <summary>
/// sobrecarga del operador '*' para la multiplicación de un vector por un escalar
/// </summary>
/// <param name="scalar">el valor del escalar tipo 'double'</param>
/// <param name="vec"> el objeto tipo 'Vector'</param>
/// <returns></returns>
		public static Vector operator * (double scalar , Vector vec) //multiplicación por un escalar
			{

			double x = vec.X * scalar;
			double y = vec.Y * scalar;

			return new Vector(x, y);
		}
	}
}

