using System;
namespace Interpreter
{
	public class Equation
	{
		 public Point? Point;
		 private readonly Constrains? _angleConstrain;
		 public bool IsPoint { get; set; }
		 public double AX { get; }
		 public double BY { get; }
		  public double C { get; }
		 public Constrains? ConstrainsX { get; }
		 public Constrains? ConstrainsY { get; }
		   public bool IsCircle { get; }
			public Constrains AngleConstrain => IsCircle ? _angleConstrain! : new("angle", 0, 2 * Math.PI);
		
		public Equation(Point point) //ecuacion del punto
		{
			IsPoint = true;
			Point = point;
		}
		
		 public Equation(double ax, double bx, double c, bool isCircle = false) //ecuacion generica
		{
			AX = ax; BY = bx; C = c;
			
			IsCircle = isCircle;
		}
			
		 public Equation(double ax, double bx, double c, 
		 				Constrains? constrainX = null, 	
						Constrains? constrainY = null)  //ecuacion de la línea, el rayo y el segmento
		{
			AX = ax; BY = bx; C = c;
			
			ConstrainsX = constrainX; ConstrainsY = constrainY;
			
			IsCircle = false;        
		}
			
			
	   
	   public Equation(double ax, double bx, double c, 
	   					Constrains? constrainAngle = null) //ecuación del arco
		{
			AX = ax; BY = bx; C = c;
			
			_angleConstrain = constrainAngle is null? new("angle", -Math.PI, Math.PI) : constrainAngle;
			
			IsCircle = true;
		}
	
	 public static Equation GetLineEquation(Point p1, Point p2, 
	 										Constrains? constrainsX = null, 
											Constrains? constrainsY = null) //devuelve la ecuacion de la linea/rayo/segmento
	{
		//  puntos por los que pasa la recta
		double x1 = p1.X;
		double y1 = p1.Y;

		double x2 = p2.X;
		double y2 = p2.Y;

		//  coeficientes 'A' , 'B' y 'C' de la ecuación
		double A = y2 - y1;
		double B = x1 - x2;
		double C = (x2 * y1) - (x1 * y2);

		return new(A, B, C, constrainsX, constrainsY);
	}
	
	public static Equation GetCircleEquation(Point center, Measure radius, 
											Constrains? angleConstrain = null) //devuelve la ecuacion de la circunferencia/arco
	{
		//centro de la circunferencia/arco
		double h = center.X;
		double k = center.Y;
		
		//radio de la circunferencia/arco
		double r = (double)radius.Value;
		
		//
		double AX = -2 * h;
		double BY = -2 * k;
		double c = h * h + k * k - r * r;
		
		return new(AX, BY, c, angleConstrain);
	}
	
	
	}
	
	public class Constrains
	{
		public string? ParameterName { get; }
		public double LowerLimit { get; }
		public double UpperLimit { get; }
		 public bool IsInRange(double parameter) => parameter >= LowerLimit && parameter <= UpperLimit;
		 
		 public static bool AcceptConstrains(double parameter , Constrains?[] constrains)
		 {
		 	foreach(var constrain in constrains)
			{
				if(constrain is not null)
				{
					if(!constrain.IsInRange(parameter)) return false;
				}
			}
			return true;	
		 }
		 
		  public Constrains(string parameterName, double lowerLimit, double upperLimit)
	{
		ParameterName = parameterName;
		
		LowerLimit = lowerLimit < upperLimit ? lowerLimit : upperLimit;
		
		UpperLimit = upperLimit > lowerLimit ? upperLimit : lowerLimit;
	}
	
	
	
	}
}


		
