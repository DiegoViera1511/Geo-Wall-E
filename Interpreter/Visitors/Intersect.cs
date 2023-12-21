using System;
namespace Interpreter
{
	public class Intersect
	{
        public Object? Evaluate(Evaluator evaluator, List<Figure?> arguments)
        {
            if (arguments.Count != 2)
                throw new InvalidOperationException();

            Figure? argument1 = arguments[0];
            Figure? argument2 = arguments[1];

            if (argument1 is null || argument2 is null)
                throw new InvalidOperationException();

           
                Equation equation1 = argument1.Equation;

                Equation equation2 = argument2.Equation;

                List<Point> points = new (Solver.SolveCircularSystem(equation1, equation2));

            return points;
           
           
        }
    }
}

