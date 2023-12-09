namespace Interpreter
{
    public abstract class InterpreterErrors : Exception 
    {
        public abstract string PrintError() ;
    }

    public class LexicalErrors : InterpreterErrors
    {
        private string badToken ;
        private int line ;

        public LexicalErrors(string badToken , int line)
        {
            this.badToken = badToken ;
            this.line = line ;

        }
        public override string PrintError()
        {
            return $"! LEXICAL ERROR: '{badToken}' is not a valid token (line :{line})." ;
        }
    }

    public enum SyntaxErrorType
    {
        MissingToken , MissingEOF , DoNotExistID
    }

    public class SyntaxErrors : InterpreterErrors
    {
        private SyntaxErrorType errorType ;
        private string? token ;
        private int? line ;
        private string? expression ;

        public SyntaxErrors(SyntaxErrorType errorType , string expression , string token , int line)
        {
            this.errorType = errorType ;
            this.expression = expression ;
            this.token = token ;
            this.line = line ;
        }
        
        public SyntaxErrors(SyntaxErrorType errorType  ,string token , int line)
        {
            this.errorType = errorType ;
            this.token = token ;
            this.line = line ;
        }

        public override string PrintError()
        {
            if( errorType == SyntaxErrorType.MissingToken )
            {
                return $"! SYNTAX ERROR: Missing ' {token} ' in expression (line : {line})." ;
            }
            else if(errorType == SyntaxErrorType.MissingEOF)
            {
                return $"! SYNTAX ERROR: Missing ' {token} ' (line : {line})";
            }
            else if(errorType == SyntaxErrorType.DoNotExistID)
            {
                return $"! SYNTAX ERROR: The name '{token}' does not exist in the current context (line : {line})";
            }
            return "";
        }

    }

    public class DefaultError : InterpreterErrors
    {
        private string errorMessage ;

        public DefaultError(string errorMessage)
        {
            this.errorMessage = errorMessage ;
        }
        public override string PrintError()
        {
            return errorMessage ;
        }
    }

    public class UnexpectedToken : InterpreterErrors
    {
        public string BadToken ;
        public UnexpectedToken(string BadToken)
        {
            this.BadToken = BadToken ;
        }
        public override string PrintError()
        { 
            return $"! ERROR : Unexpected Token '{BadToken}'";
        }
    }

    public class InvalidID : InterpreterErrors
    {
        private string badID ;

        private int line ;

        public InvalidID(string badID , int line)
        {
            this.badID = badID ;
            this.line = line ;
        }
        public override string PrintError()
        {
            if(Lexer.KeyWordsList.Contains(badID))
            {
                return $"! SYNTAX ERROR: Invalid ID , the name '{badID}' it's a keyword language (line : {line})";
            }
            else return $"! SYNTAX ERROR: the name {badID} is not a valid ID (line : {line})";
        }
    }

    class ConditionalErrors : SemanticErrors
    {
        public string badToken ;
        private int line ;
        public ConditionalErrors(string badToken , int line)
        {
            this.badToken = badToken ;
            this.line = line ;
        }
        public override string PrintError()
        {
            return $"! SEMANTIC ERROR: ' if ' expression receives boolean not {badToken} (line : {line})";
        }
    }

    public abstract class SemanticErrors : InterpreterErrors {}

    public class IncorrectBinaryExpression : SemanticErrors
    {
        public string operatorProblem ;
        public string left ;
        public string right ;
        public int line ;
        public ExpressionType expectedType ;

        public ExpressionType badType ;

        public IncorrectBinaryExpression(string operatorProblem  , ExpressionType expectedType , ExpressionType badType , string left , string right , int line )
        {
            this.left = left ;
            this.right = right ;
            this.operatorProblem = operatorProblem ;
            this.line = line ;
            this.expectedType = expectedType ;
            this.badType = badType ;
        }

        public override string PrintError()
        {   
            return $"! SEMANTIC ERROR: {operatorProblem} cannot be used between '{left}' and '{right}' (line : {line}).";
        }
    }

    public class ArgumentTypeError : SemanticErrors
    {
        string functionName ;
        public string expectedToken ;
        public string badToken ;

        public int line ;

        public ArgumentTypeError(string expectedToken , string badToken  , int line, string functionName)
        {
            this.functionName = functionName ;
            this.expectedToken = expectedToken ;
            this.badToken = badToken ;
            this.line = line ;
        }

        public override string PrintError()
        {
            return $"! SEMANTIC ERROR: Function '{functionName}' receives '{expectedToken}', not '{badToken}'.";
        }
    }
    public class ArgumentCountError : SemanticErrors
    {
        private string functionName ;
        private int argumentsIdCount ;
        private int argumentsValueCount ;

        public ArgumentCountError(string functionName , int argumentsIdCount , int argumentsValueCount)
        {
            this.functionName = functionName ;
            this.argumentsIdCount = argumentsIdCount ;
            this.argumentsValueCount = argumentsValueCount ;
        }

        public override string PrintError()
        {
           return $"! SEMANTIC ERROR: Function '{functionName}' receives {argumentsIdCount} argument(s), but {argumentsValueCount} were given." ;
        }
    }

    class DuplicateArgument : SemanticErrors
    {
        private string badToken ;
        private int line ;
        public DuplicateArgument(string badToken , int line)
        {
            this.badToken = badToken ;
            this.line = line;
        }

        public override string PrintError()
        {
            return $" ! SEMANTIC ERROR: The parameter name '{badToken}' is a duplicate (line : {line})";
        }

    }

    class IncorrectOperator : SemanticErrors
    {
        public string operatorProblem ;
        public string badToken ;
        public int line ;

        public ExpressionType expectedType ;
        public IncorrectOperator(string badToken , ExpressionType expectedType , string operatorProblem , int line)
        {
            this.badToken = badToken ;
            this.operatorProblem = operatorProblem ;
            this.line = line ;
            this.expectedType = expectedType ;
        }

        public override string PrintError()
        {
            return $"! SEMANTIC ERROR: {operatorProblem} cannot be applied to operand of type '{badToken}' (line : {line}).";
        }
    }

    

}