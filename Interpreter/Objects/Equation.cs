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
			public Constrains AngleConstrain => IsCircle ? _angleConstrain! : new( 0, 2 * Math.PI);
		
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
			
				_angleConstrain = constrainAngle is null? new( -Math.PI, Math.PI) : constrainAngle;
			
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
	
	public static Equation GetCircleEquation(Point center, double radius, 
											Constrains? angleConstrain = null) //devuelve la ecuacion de la circunferencia/arco
	{
		//centro de la circunferencia/arco
		double h = center.X;
		double k = center.Y;
		
		//radio de la circunferencia/arco
		double r = radius;
		
		//
		double AX = -2 * h;
		double BY = -2 * k;
		double c = h * h + k * k - r * r;
		
		return new(AX, BY, c, angleConstrain);
	}
	
	
	}
	
	public class CuadraticEquation
	
	{
		public double AX2 { get; }
		public double BX { get; }
		public double C { get; }
	
		public CuadraticEquation(double a, double b, double c)
		{
			AX2 = a;
			BX = b;
			C = c;
		}
		public List<double> SolveEquation()
		{
			List<double> result = new();

			double discriminant = BX * BX - 4 * AX2 * C;
	
			if(discriminant > 0) // caso de dos soluciones
			{
				double x1 = (-BX + Math.Sqrt(discriminant)) / (2 * AX2);
				double x2 = (-BX - Math.Sqrt(discriminant)) / (2 * AX2);
			
				result.Add(x1);
				result.Add(x2);
			
				return result;
			}
			
			if(discriminant == 0) // caso de una sola solución
			{
				double x = -BX / (2 * AX2);
			
				result.Add(x);
			
				return result;
			}
			
			return result;
		}
    }
	
	public class Constrains
	{
		
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
		 
		  public Constrains( double lowerLimit, double upperLimit)
			{
		
		
				LowerLimit = lowerLimit < upperLimit ? lowerLimit : upperLimit;
		
				UpperLimit = upperLimit > lowerLimit ? upperLimit : lowerLimit;
			}
	
	
	
	}
	
	public class Solver
	{
		/// <summary>
		/// filtro mis restricciones para quitar casos fuera de mi dominio (aplica para todas las figuras menos linea, circulo y punto)
		/// </summary>
		/// <param name="PosibleSolutions">mis supuestas soluciones</param>
		/// <param name="circle_st"> circulo 1 </param>
		/// <param name="circle_nd"> circulo 2 </param>
		/// <returns></returns>
        private static IEnumerable<Point> Filter(IEnumerable<Point> PosibleSolutions, Equation circle_st, Equation circle_nd)
        {
            List<Point> result = new();

			foreach (var point in PosibleSolutions)
            {
                double x = point.X;
                double y = point.Y;
                Constrains?[] constrainsX = { circle_st.ConstrainsX, circle_nd.ConstrainsX };

				if (Constrains.AcceptConstrains(x, constrainsX))
                    continue;

				Constrains?[] constrainsY = { circle_st.ConstrainsY, circle_nd.ConstrainsY };

				if (Constrains.AcceptConstrains(y, constrainsY))
                    continue;

				if (circle_st.IsCircle)
                {
                    double xc = circle_st.AX / (-2);
                    double yc = circle_st.BY / (-2);
                    double theta = Math.Atan2(y - yc, x - xc);
					Constrains thetaConstrain = circle_st.AngleConstrain;

					if (!thetaConstrain.IsInRange(theta) && !thetaConstrain.IsInRange(theta + 2 * Math.PI))
                        continue;
                }

				if (circle_nd.IsCircle)
                {
                    double xc = circle_nd.AX / (-2);
                    double yc = circle_nd.BY / (-2);
                    double theta = Math.Atan2(y - yc, x - xc);
                    var thetaConstrain = circle_nd.AngleConstrain;

					if (!thetaConstrain.IsInRange(theta) && !thetaConstrain.IsInRange(theta + 2 * Math.PI))
                        continue;
                }

                result.Add(point);
            }

            return result;
        }

		/// <summary>
		/// calculo los coeficientes y resuelvo el sistema de ecuaciones
		/// </summary>
		/// <param name="lineal_st"> primera ecuacion lineal </param>
		/// <param name="lineal_nd"> segunda ecuacion lineal </param>
		/// <returns></returns>
		private static List<Point> SolveLineal(Equation lineal_st, Equation lineal_nd)
        {
            
            double a1 = lineal_st.AX;
            double b1 = lineal_st.BY;
            double c1 = -lineal_st.C;

            double a2 = lineal_nd.AX;
            double b2 = lineal_nd.BY;
            double c2 = -lineal_nd.C;

            double determinant = a1 * b2 - a2 * b1;
         
            if (determinant == 0) return new();

            double x = (b2 * c1 - b1 * c2) / determinant;
            double y = (a1 * c2 - a2 * c1) / determinant;

            List<Point> result = new() { new Point(x , y) };

			return result;
        }

		/// <summary>
		/// resuelvo dos ecuaciones cuales quiera sea la combinacion,
		/// en dependencia de cuales sean llamo a los respectivos metodos.
		/// </summary>
		/// <param name="circle_st">primera equacion</param>
		/// <param name="circle_nd">segunda equacion</param>
		/// <returns></returns>
		public static IEnumerable<Point> SolveCircularSystem(Equation circle_st, Equation circle_nd)
        {
            List<Point> results;

			if (!circle_st.IsCircle && !circle_nd.IsCircle)
                results = SolveLineal(circle_st, circle_nd);

			else if (!circle_st.IsCircle)
                results = SolveCircleAndLineal(circle_nd, circle_st);

			else if (!circle_nd.IsCircle)
                results = SolveCircleAndLineal(circle_st, circle_nd);
            else
            {
                double a = circle_st.AX - circle_nd.AX;
                double b = circle_st.BY - circle_nd.BY;
                double c = circle_st.C - circle_nd.C;

                Equation equation = new(a, b, c, false);

                results = SolveCircleAndLineal(circle_st, equation);
            }
            return Filter(results, circle_st, circle_nd);
        }


		/// <summary>
		/// este metodo resuelve la interseccion entre dos figuras de tipo (arco/circulo) con (segmento/rayo/linea)
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="lineal"></param>
		/// <returns></returns>
		public static List<Point> SolveCircleAndLineal(Equation circle, Equation lineal)
        {
            double a = lineal.AX;
            double b = lineal.BY;
            double c = lineal.C;

            if (a == 0 && b == 0) return new();

            CuadraticEquation cuadratic;

            List<Point> results = new();

			if (a == 0)
            {
                double y = -c / b;
                var aloneY = new Equation(0, 0, y, false);
                cuadratic = Substitute(aloneY, circle, false);
				List<double> xi = cuadratic.SolveEquation();

                foreach (double x in xi)
                {
                    results.Add(new(x , y));
                }

                return results;
            }
            var aloneX = new Equation(0 , -b/a , -c/a , false);

            cuadratic = Substitute(aloneX, circle, true); 

            List<double> y_ = cuadratic.SolveEquation();

			if (b == 0)
            {
                double x = -c / a;

                foreach (double y in y_)
				{
                    results.Add(new(x, y));
                }
                  
                return results;
            }

            foreach (double y in y_)
            {
                double x = -(b * y + c) / a;
                results.Add(new(x , y));
            }
            return results;
        }

		/// <summary>
		/// este metodo es para hacer la "sustitucion" de mi variable en mi otra ecuacion,
		///como la via de hacerlo en papel de toda la vida
		/// </summary>
		/// <param name="parameter">la q voy a sustituir</param>
		/// <param name="equation">donde voy a hacer la sustitucion</param>
		/// <param name="isXval">valor booleano</param>
		/// <returns></returns>
		private static CuadraticEquation Substitute(Equation parameter, Equation equation, bool isXval)
        {
            double a;
            double b;
            double c;

            if (isXval)
            {
                a = 1 + Math.Pow(parameter.BY, 2);
                b = 2 * parameter.BY * parameter.C + equation.AX * parameter.BY + equation.BY;
                c = Math.Pow(parameter.C, 2) + equation.AX * parameter.C + equation.C;
            }
            else
            {
                a = 1 + Math.Pow(parameter.AX, 2);
                b = 2 * parameter.AX * parameter.C + equation.BY * parameter.AX + equation.AX;
                c = Math.Pow(parameter.C, 2) + equation.BY * parameter.C + equation.C;
            }
            return new CuadraticEquation(a, b, c);
        }


    }
}