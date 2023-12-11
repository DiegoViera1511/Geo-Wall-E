using System;
namespace Interpreter
{
	public class GSharpInterpreter
	{
		Parser InterpreterParser = new Parser();
		public static List<Figure> GraficsViewPrints { get { return Parser.Prints; } }
		public GSharpInterpreter()
		{
			Parser.ResetParser();
		}

		public void RunInterpreter(string input)
		{
			List<IExpression> expressions = InterpreterParser.ParseExpression(input);
			InterpreterParser.Execute(expressions);
		}
	}
}

