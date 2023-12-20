using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Interpreter
{
	public abstract class Figure 
	{
		public abstract Color FigureColor { get; }

        public abstract string? Text { get; set; }
		
	}
}