using System;
namespace Interpreter
{
	public class Equation
	{
		public Point point;
		
		public Equation()
		{
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

