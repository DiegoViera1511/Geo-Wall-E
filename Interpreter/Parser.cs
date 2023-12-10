using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Interpreter
{
    public enum ExpressionType
    {
        Number , String , Boolean , Inference , Figure , Sequence , Undefined , Statement
    }

    public enum Color
    {
        Blue , Red , Yellow , Green , Cyan , Magenta , White , Gray , Black
    }

    public class Context
    {
        public Dictionary< string , Expression > Variables = new();
        public Dictionary<string , object> FunctionsArgumentsValue = new();
        public Dictionary<string , Figure> FiguresVariables = new();

        

        public void AddVariable(  string name , object variableValue)
        { 
            if(FiguresVariables.ContainsKey(name))
            {
                if(FiguresVariables[name].GetType() == variableValue.GetType())
                {
                    FiguresVariables[name] = (Figure)variableValue ;
                }
                else throw new Exception("Error no se puede asignar una variable a valores de diferente tipo");
            }
            else if(Variables.ContainsKey(name))
            {   
                Analizer checkType = new Analizer();
                Expression expr = (Expression)variableValue ;
                if(Variables[name].Accept(checkType).Equals(expr.Accept(checkType)) )
                {
                    Variables[name] = expr ;
                }
                else throw new Exception("Error no se puede asignar una variable a valores de diferente tipo");
            }
            if(variableValue is Figure)
            {
                FiguresVariables.Add( name ,(Figure)variableValue);
            }
            else
            {
                Variables.Add(name , (Expression)variableValue) ;
            }
        }

        public bool ExistID(string name)
        {
            if(Parser.CurrentContext.FunctionsArgumentsValue.ContainsKey(name))
            {
                return true ;
            }
            else if(Parser.CurrentContext.Variables.ContainsKey(name))
            {
                return true ;
            }
            else if(Parser.CurrentContext.FiguresVariables.ContainsKey(name))
            {
                return true ;
            }
            else
            {
                return false ;
            }
        }
    }
    
    public class Parser
    {
        
        public static int ActualLine = 0 ;
        public static int GetLine {get => ActualLine + 1;}
        public static int Index = 0 ;
        public static List<List<Token>> Lines = new() ;

        public static Context GlobalVariables = new();
        public static Stack<Context> Contexts = new ();
        public static Stack<Color> ColorsStack = new();
        public static Color actualColor { get { if (ColorsStack.Count == 0) return Color.Black; else return ColorsStack.Peek(); } }
        public static List<Figure> Prints = new();
        public  static Stack<string> AnalizeFunctionsStack = new();
        public static Token CurrentToken
        {
            get
            {
                if (EndOfFile())
                {
                    throw new DefaultError($"! ERROR: Missing expression (line : {ActualLine - 1})");
                }
                while (Lines[ActualLine].Count == 0 || Index >= Lines[ActualLine].Count)
                {
                    Next();
                    if (EndOfFile())
                    {
                        throw new DefaultError($"! ERROR: Missing expression (line : {ActualLine - 1})");
                    }
                }
                return Lines[ActualLine][Index] ;
            }
        }

        public static Context CurrentContext
        {
            get { if (Contexts.Count == 0) return GlobalVariables ; else return Contexts.Peek();}
        }

        public static void Next() 
        {   
            Index += 1 ;
            if(Index >= Lines[ActualLine].Count)
            {
                Index = 0 ;
                ActualLine += 1 ;
                if(ActualLine == Lines.Count)
                {
                    return ;
                }
            }
        }

        public static void ResetParser()
        {
            Function.FunctionStore.Clear();
            GlobalVariables.FiguresVariables.Clear();
            GlobalVariables.Variables.Clear();
            StatementExpression.VariableDeclaration.DeclaredVariables.Clear();
            GlobalVariables.FunctionsArgumentsValue.Clear();
            AnalizeFunctionsStack.Clear();
            Contexts.Clear();
            Prints.Clear();
            ColorsStack.Clear();
            ActualLine = 0;
            Index = 0;
        }
        public static Token Previous()
        {
            if(Index - 1 >= 0)
            {
                return Lines[ActualLine][Index - 1] ;
            }
            else return Lines[ActualLine - 1][Lines[ActualLine - 1].Count - 1];
        }

        public static bool MatchToken(TokenType tokentype)
        {
            return CurrentToken.TypeOfToken == tokentype ;
        }

        public static bool EndOfFile()
        {
            return ActualLine >= Lines.Count ;
        }
        
        public static ExpressionType GetObjectType(object exprValue)
        {
            if(exprValue.GetType() == Type.GetType("System.Double"))
            {
                return ExpressionType.Number ;
            }
            else if(exprValue.GetType() == Type.GetType("System.String"))
            {
                return ExpressionType.String ;
            }
            else if(exprValue.GetType() == Type.GetType("System.Boolean"))
            {
                return ExpressionType.Boolean ;
            }
            else 
            {
                return ExpressionType.Figure ;
            }
        }

        public void Evaluate(IExpression expr)
        {
            expr.Accept(new Evaluator()) ;
        }

        public void Analize(IExpression expr)
        {
            expr.Accept(new Analizer());
        }

        public void Execute(List<IExpression> exprssions)
        {
            foreach(IExpression expr in exprssions)
            {
                Analize(expr);
                Evaluate(expr);
            }
        }

        public List<IExpression> ParseExpression(string input)
        {
            ResetParser();
            Lines = Lexer.Tokenize(input);
            List<IExpression> expressions = new();
            if(EndOfFile()) return expressions ;
            while (!EndOfFile())
            {
                if (Lines[ActualLine].Count == 0)
                {
                    ActualLine += 1;
                    continue;
                }
                if (MatchToken(TokenType.LET))
                {
                    Next();
                    expressions.Add(LetInExpression());
                }
                else if(MatchToken(TokenType.IF))
                {
                    Next();
                    expressions.Add(ConditionalExpression());
                }
                else if(MatchToken(TokenType.DRAW))
                {
                    Next();
                    expressions.Add(DrawStmts());
                }
                else if(MatchToken(TokenType.COLOR))
                {
                    Next();
                    expressions.Add(ColorStmts());
                }
                else if(MatchToken(TokenType.RESTORE))
                {
                    Next();
                    expressions.Add(new StatementExpression.Restore());
                }
                else if(MatchToken(TokenType.POINT))
                {
                    Next();
                    expressions.Add(RandomPointStmts());
                }
                else if(MatchToken(TokenType.CIRCLE))
                {
                    Next();
                    expressions.Add(RandomCircleStmts());
                }
                else if(MatchToken(TokenType.LINE) || MatchToken(TokenType.SEGMENT) || MatchToken(TokenType.RAY))
                {
                    expressions.Add(RandomLineStmts());
                }
                else if(MatchToken(TokenType.IMPORT))
                {
                    Next();
                    expressions.Add(ImportStmts());
                }
                else if(MatchToken(TokenType.ID))
                {
                    if (StatementExpression.FunctionDeclaration.DeclaredFunctions.Contains(CurrentToken.StringForm))
                    {
                        expressions.Add(FunctionCallExpression());
                    }
                    else if (StatementExpression.VariableDeclaration.DeclaredVariables.Contains(CurrentToken.StringForm))
                    {
                        expressions.Add(NewExpression());
                    }
                    else
                    {
                        Next();
                        if (MatchToken(TokenType.LEFT_PARENTHESIS))
                        {
                            expressions.Add(FunctionDeclarationStmst());
                        }
                        else if (MatchToken(TokenType.EQUAL))
                        {
                            expressions.Add(VariableDeclarationStmts());
                        }
                        else if (MatchToken(TokenType.COMMA))
                        {
                            expressions.Add(VariableSequenceDeclarationStmts());
                        }
                        else
                        {
                            throw new UnexpectedToken(CurrentToken.StringForm);
                        }
                    }
                }
                else if(MatchToken(TokenType.SKIP))
                {
                    Next();
                    expressions.Add(VariableSequenceDeclarationStmts()) ;
                }
                else 
                {
                    expressions.Add(NewExpression());
                }
                if(EndOfFile() || !MatchToken(TokenType.EOF))
                {
                    throw new SyntaxErrors( SyntaxErrorType.MissingEOF, ";" , GetLine );
                }
                else Next();
            }
            return expressions ;
        } 


        #region Expressions Parser
        
        public static Expression NewExpression()
        {
            return LogicTerm();
        }

        public static  Expression LogicTerm()
        {
            List<string> NextTokens = new List<string>(){")", "}",";",",","in","else","then","..."};
            Expression left = ComparisonTerm();
            while(!EndOfFile())
            {
                if(MatchToken(TokenType.AND) || MatchToken(TokenType.OR))
                {
                    Token operation = CurrentToken ;
                    Next();
                    Expression right = ComparisonTerm();
                    if(operation.TypeOfToken == TokenType.AND)
                    {
                        left = new  LogicAND(left , right);
                    }
                    else left = new LogicOR(left , right);
                }
                else if(NextTokens.Contains(CurrentToken.StringForm))
                {
                    break ;
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
            }
            return left ;
        }

        public static Expression EqualityTerm()
        {
            List<string> NextTokens = new List<string>(){")","}",";",",","in","else","&","|","then","..." };
            Expression left = ComparisonTerm();
            while(!EndOfFile())
            {
                if(MatchToken(TokenType.EQUAL_EQUAL) || MatchToken(TokenType.NOT_EQUAL))
                {
                    Token operation = CurrentToken;
                    Next();
                    Expression right = ComparisonTerm();
                    if(operation.TypeOfToken == TokenType.EQUAL_EQUAL)
                    {
                        left = new Equality(left , right);
                    }
                    else left = new NotEquality(left , right);
                }
                else if(!EndOfFile() && NextTokens.Contains(CurrentToken.StringForm))
                {
                    break;
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
            }
            return left ;
        }

        public static Expression ComparisonTerm()
        {
            List<string> NextTokens = new List<string>(){")","}",";",",","in","else","&","|","then","...","==","!=" };
            Expression left = Term();
            while(!EndOfFile())
            {
                if(MatchToken(TokenType.LESS) || MatchToken(TokenType.GREATER) || MatchToken(TokenType.LESS_EQUAL) || MatchToken(TokenType.GREATER_EQUAL))
                {
                    
                    Token operation = CurrentToken;
                    Next();
                    Expression right = Term();
                    if(operation.TypeOfToken == TokenType.LESS)
                    {
                        left = new ComparisonLESS(left , right);
                    }
                    else if(operation.TypeOfToken == TokenType.GREATER)
                    {
                        left = new ComparisonGREATER(left , right);
                    }
                    else if(operation.TypeOfToken == TokenType.LESS_EQUAL)
                    {
                        left = new ComparisonLESS_EQUAL(left , right);
                    }
                    else
                    {
                        left = new ComparisonGREATER_EQUAL(left , right);
                    }
                }
                else if(!EndOfFile() && NextTokens.Contains(CurrentToken.StringForm))
                {
                    break;
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
            }
            return left ;
        }

        public static Expression Term()
        {
            List<string> NextTokens = new List<string>(){";",")", "}","in",",",">","<","else","<","<=",">=","&","|","==","!=","then","..."};
            Expression left = Factor();
            while(!EndOfFile())
            {
                if(MatchToken(TokenType.PLUS) || MatchToken(TokenType.MINUS))
                {
                    Token operation = CurrentToken ;
                    Next();
                    Expression right = Factor();
                    if(operation.TypeOfToken == TokenType.PLUS)
                    {
                        left = new Addition(left , right);
                    }
                    else left = new Subtraction(left , right);
                }
                else if(NextTokens.Contains(CurrentToken.StringForm))
                {
                    break ;
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
            }
            return left ;
        }

        public static Expression Factor()
        {
            List<string> NextTokens = new List<string>(){";", ")" , "}" ,"in",",",">","<","else","<","<=",">=","&","|","==","!=","+","-","then","..."};
            Expression left = PowerTerm();
            while(!EndOfFile())
            {
                if( MatchToken(TokenType.MULTIPLICATION) || MatchToken(TokenType.DIVISION) || MatchToken(TokenType.MODULE))
                {
                    Token operation = CurrentToken ;
                    Next();
                    Expression right = PowerTerm();
                    if(operation.TypeOfToken == TokenType.MULTIPLICATION)
                    {
                        left = new Multiplication(left , right);
                    }
                    else if(operation.TypeOfToken == TokenType.DIVISION)
                    {
                        left = new Division(left , right);
                    }
                    else left = new Module(left , right);
                }
                else if(NextTokens.Contains(CurrentToken.StringForm))
                {
                    break ;
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
            }
            return left ;
        }

        public static Expression PowerTerm()
        {
            List<string> NextTokens = new List<string>(){";",")","}","in",",",">","<","else","<=",">=","&","|","==","!=","+","-","*","/","%","then","..."};
            Expression left = AtomTerm();
            while(!EndOfFile())
            {
                if(MatchToken(TokenType.POWER))
                {
                    Next();
                    Expression right = AtomTerm();
                    left = new Power(left , right);
                }
                else if (NextTokens.Contains(CurrentToken.StringForm))
                {
                    break ;
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
            }
            return left ;
        }

        public static Expression AtomTerm()
        {
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                Expression expr = NewExpression();
                if(MatchToken(TokenType.RIGHT_PARENTHESIS))
                {
                    Next();
                    return expr ;
                }
                else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "Atom" , ")" , GetLine);
            }
            else if(MatchToken(TokenType.MINUS) || MatchToken(TokenType.NOT)) 
            {
                Token operation = CurrentToken ;
                Next();
                Expression right = AtomTerm();
                if(operation.TypeOfToken == TokenType.MINUS)
                {
                    return new Negative(right);
                }
                else return new Negation(right);
            }
            else if(MatchToken(TokenType.NUMBER))
            {
                Expression expr = new Expression.Atom.Number(Convert.ToDouble(CurrentToken.StringForm)) ;
                Next();
                return expr ;
            }
            else if(MatchToken(TokenType.STRING))
            {
                Expression expr = new Expression.Atom.String(CurrentToken.StringForm.Substring( 1 , CurrentToken.StringForm.Length - 2));
                Next();
                return expr ;
            }
            else if(MatchToken(TokenType.BOOLEAN))
            {
                Expression expr = new Expression.Atom.Boolean(Convert.ToBoolean(CurrentToken.StringForm));
                Next();
                return expr ;
            }
            else if(MatchToken(TokenType.LET))
            {
                Next();
                return LetInExpression();
            }
            else if(MatchToken(TokenType.IF))
            {
                Next();
                return ConditionalExpression();
            }
            else if(MatchToken(TokenType.POINT))
            {
                Next();
                return PointDeclarationExpression();
            }
            else if(MatchToken(TokenType.LINE) || MatchToken(TokenType.SEGMENT) || MatchToken(TokenType.RAY))
            {
                return LineDeclarationExpression();
            }
            else if(MatchToken(TokenType.ARC))
            {
                Next();
                return ArcDeclarationExpression();
            } 
            else if(MatchToken(TokenType.CIRCLE))
            {
                Next();
                return CircleDeclarationExpression();
            }
            else if(MatchToken(TokenType.MEASURE))
            {
                Next();
                return MeasureExpr();
            }
            else if(MatchToken(TokenType.LEFT_BRACE))
            {
                Next();
                return SequenceExpression();
            }
            else if(MatchToken(TokenType.ID))
            {
                if(StatementExpression.VariableDeclaration.DeclaredVariables.Contains(CurrentToken.StringForm))
                {
                    string name = CurrentToken.StringForm;
                    Next();
                    return new Expression.ID( name , false ) ;
                }
                if(StatementExpression.FunctionDeclaration.DeclaredFunctions.Contains(CurrentToken.StringForm))
                {
                    return FunctionCallExpression();
                }
                else if(CurrentContext.Variables.ContainsKey(CurrentToken.StringForm) && Parser.CurrentContext.Variables[CurrentToken.StringForm] is Expression.ID)
                {
                    Expression.ID idfunction = (Expression.ID)CurrentContext.Variables[CurrentToken.StringForm];
                    if(idfunction.IsFunctionName)
                    {
                        return CheckFunctionCall() ;
                    }
                    else 
                    {
                        Next();
                        return idfunction ;
                    }
                }
                else throw new SyntaxErrors(SyntaxErrorType.DoNotExistID , CurrentToken.StringForm , GetLine);
            }
            else throw new UnexpectedToken(CurrentToken.StringForm);
        }

        public static Expression CheckFunctionCall()
        {
            int argumentCount = 0 ;
            string functionName = CurrentToken.StringForm;
            List<Expression> ParametersValue = new();
            Next();
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                while(!EndOfFile() && ! MatchToken(TokenType.RIGHT_PARENTHESIS))
                {
                    argumentCount += 1 ;
                    ParametersValue.Add(NewExpression());
                    if(! EndOfFile())
                    {
                        if(! MatchToken(TokenType.COMMA))
                        {
                            break ;
                        }
                        else if(MatchToken(TokenType.ID))
                        {
                            throw new SyntaxErrors( SyntaxErrorType.MissingToken , "function call" , "," , GetLine ); 
                        }
                        else Next();
                    }
                }
                if(!EndOfFile() && MatchToken(TokenType.RIGHT_PARENTHESIS))
                {
                    Next();
                }
                else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function call" , ")" , GetLine);
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function call" , "(" , GetLine);
            
            if(CurrentContext.Variables.Count - 1 != ParametersValue.Count)
            {
                throw new ArgumentCountError(functionName , CurrentContext.Variables.Count - 1 , ParametersValue.Count);
            }
            else 
            {
                return new Expression.FunctionCall(functionName , ParametersValue);
            }
        }

        public static Expression SequenceExpression()
        {
            List<Expression> sequenceValues = new();
            Expression value1 = NewExpression();
            sequenceValues.Add(value1);
            if(MatchToken(TokenType.THREE_DOTS))
            {
                Expression.Atom.Number number1 ;
                if(value1 is Expression.Atom.Number)
                {
                    number1 = (Expression.Atom.Number)value1;
                }
                else throw new DefaultError($"! ERROR: infinity sequences and range sequences contains numbers (line : {GetLine})");
                Next();
                if(MatchToken(TokenType.RIGHT_BRACE))
                {
                    Next();
                    return new InfinitySequence((double)number1.Value);
                }
                else 
                {
                    Expression value2 = NewExpression();
                    Expression.Atom.Number number2 ;
                    if(value2 is Expression.Atom.Number)
                    {
                        number2 = (Expression.Atom.Number)value2;
                    }
                    else throw new DefaultError($"! ERROR: infinity sequences and range sequences contains numbers (line : {GetLine})");
                    if(MatchToken(TokenType.RIGHT_BRACE))
                    {
                        Next();
                        return new RangeSequence( (double)number1.Value ,(double)number2.Value );
                    }
                    else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "Sequence" , "}" , GetLine);
                }
            }
            else if(MatchToken(TokenType.COMMA))
            {
                Next();
                while(!EndOfFile())
                {
                    Expression value = NewExpression();
                    sequenceValues.Add(value);
                    if(MatchToken(TokenType.COMMA))
                    {
                        Next();
                    }
                    else if(MatchToken(TokenType.RIGHT_BRACE))
                    {
                        break ;
                    }
                    else throw new UnexpectedToken(CurrentToken.StringForm);
                }
                if(!EndOfFile())
                {
                    Next();
                    return new ValuesSecquence(sequenceValues);
                }
                else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "Sequence" , "}" , GetLine);
            }
            else if(MatchToken(TokenType.RIGHT_BRACE))
            {
                Next();
                return new ValuesSecquence(sequenceValues);
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "Sequence" , "}" , GetLine);  
        }

        public static Expression LetInExpression()
        {
            Dictionary<string , IExpression> localVariables = new();
            List<StatementExpression> letInStatements = new();

            while(!EndOfFile())
            {
                StatementExpression stmts = Expression.Let_In.NewLetInStatements();
                if(stmts is StatementExpression.VariableDeclaration)
                {
                    StatementExpression.VariableDeclaration variable = (StatementExpression.VariableDeclaration)stmts ;
                    localVariables.Add(variable.VariableName , variable.Value );
                    letInStatements.Add(stmts);
                }
                else
                {
                    letInStatements.Add(stmts);
                }
                if(MatchToken(TokenType.EOF))
                {
                    Next();
                }
                else if(MatchToken(TokenType.IN))
                {
                    Next();
                    break ;
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
            }
            Expression Body = NewExpression();
            Expression.Let_In.RemoveLocalVariables(localVariables);
            return new Expression.Let_In( Body , localVariables , letInStatements );
        }

        public static Expression ConditionalExpression()
        {
            Expression IfExpression = NewExpression();

            if(MatchToken(TokenType.THEN))
            {
                Next();
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "if-then-else" , "then" , GetLine);

            Expression ThenExpression = NewExpression() ;

            if(MatchToken(TokenType.ELSE))
            {
                Next();
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "if-then-else" , "else" , GetLine);

            Expression ElseExpression = NewExpression() ;

            return new Expression.Conditional(IfExpression , ThenExpression , ElseExpression);
        }

        public static Expression PointDeclarationExpression()
        {
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                Expression x = NewExpression();
                if(MatchToken(TokenType.COMMA))
                {
                    Next();
                    Expression y = NewExpression();
                    if(MatchToken(TokenType.RIGHT_PARENTHESIS))
                    {
                        Next();
                        return new Expression.PointDeclaration( x , y ) ;
                    }
                    else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "point declaration" , ")" , GetLine);
                }
                else
                {
                    throw new SyntaxErrors(SyntaxErrorType.MissingToken , "point declaration" , "," , GetLine);
                }
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "point declaration" , "(" , GetLine);
        }
    
        public static Expression LineDeclarationExpression()
        {
            string lineType = CurrentToken.StringForm ;
            Next();
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                if(MatchToken(TokenType.ID))
                {
                    string p1Name = CurrentToken.StringForm ;
                    Next();
                    if(MatchToken(TokenType.COMMA))
                    {
                        Next();
                        string p2Name = CurrentToken.StringForm ;
                        Next();
                        if(MatchToken(TokenType.RIGHT_PARENTHESIS))
                        {
                            Next();
                            return new Expression.LineDeclaration(p1Name , p2Name , lineType);
                        }
                        else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "line declaration" , ")" , GetLine);
                    } 
                    else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "line declaration" , "," , GetLine);
                }
                else throw new InvalidID(CurrentToken.StringForm , GetLine);
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "line declaration" , "(" , GetLine);
        }
    
        public static Expression ArcDeclarationExpression()
        {
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                if(MatchToken(TokenType.ID))
                {
                    string p1Name = CurrentToken.StringForm ;
                    Next();
                    if(MatchToken(TokenType.COMMA))
                    {
                        Next();
                        string p2Name = CurrentToken.StringForm ;
                        Next();
                        if(MatchToken(TokenType.COMMA))
                        {
                            Next();
                            string p3Name = CurrentToken.StringForm ;
                            Next();
                            if(MatchToken(TokenType.COMMA))
                            {
                                Next();
                                Expression m = NewExpression();
                                if(MatchToken(TokenType.RIGHT_PARENTHESIS))
                                {
                                    Next();
                                    return new Expression.ArcDeclaration(p1Name , p2Name , p3Name , m);
                                }
                                else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "arc declaration" , ")" , GetLine);
                            }
                            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "arc declaration" , "," , GetLine);
                        }
                        else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "arc declaration" , "," , GetLine);                     
                    } 
                    else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "arc declaration" , "," , GetLine);
                }
                else throw new InvalidID(CurrentToken.StringForm, GetLine);
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "arc declaration" , "(" , GetLine);
        }

        public static Expression CircleDeclarationExpression()
        {
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                if(MatchToken(TokenType.ID))
                {
                    string centerName = CurrentToken.StringForm ;
                    Next();
                    if(MatchToken(TokenType.COMMA))
                    {
                        Next();
                        Expression m = NewExpression();
                        if(MatchToken(TokenType.RIGHT_PARENTHESIS))
                        {
                            Next();
                            return new Expression.CircleDeclaration(centerName , m);
                        }
                        else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "circle declaration" , ")" , GetLine);
                    }
                    else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "circle declaration" , "," , GetLine);
                }
                else throw new InvalidID(CurrentToken.StringForm, GetLine);
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "circle declaration" , "(" , GetLine);
        }

        public static Expression MeasureExpr()
        {
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                if(MatchToken(TokenType.ID))
                {
                    string p1Name = CurrentToken.StringForm ;
                    Next();
                    if(MatchToken(TokenType.COMMA))
                    {
                        Next();
                        if(MatchToken(TokenType.ID))
                        {
                            string p2Name = CurrentToken.StringForm ;
                            Next();
                            if(MatchToken(TokenType.RIGHT_PARENTHESIS))
                            {
                                Next();
                                return new Expression.MeasureExpression(p1Name , p2Name);
                            }
                            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "Measure" , ")" , GetLine);
                        }
                        else throw new InvalidID(CurrentToken.StringForm, GetLine);
                    }
                    else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "Measure" , "," , GetLine);
                }
                else throw new InvalidID(CurrentToken.StringForm, GetLine);
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "Measure" , "(" , GetLine);
        }

        public static Expression FunctionCallExpression()
        {
            string functionName = CurrentToken.StringForm ;
            List<Expression> ParametersValue = new();
            Next();
            if(MatchToken(TokenType.LEFT_PARENTHESIS))
            {
                Next();
                while(!EndOfFile() && ! MatchToken(TokenType.RIGHT_PARENTHESIS) )
                {
                    ParametersValue.Add(NewExpression());
                    
                    if(! EndOfFile() && ! MatchToken(TokenType.COMMA))
                    {
                        break ;
                    }
                    else if(!EndOfFile() && MatchToken(TokenType.ID))
                    {
                        throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function call" , "," , GetLine);
                    }
                    else Next();
                }
                if(!EndOfFile() && MatchToken(TokenType.RIGHT_PARENTHESIS))
                {
                    Next();
                }
                else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function call" , ")" , GetLine);
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function call" , "(" , GetLine);

            return new Expression.FunctionCall(functionName , ParametersValue);
        }

        #endregion

        #region Statements Parser
        
        public static StatementExpression DrawStmts()
        {
            Expression figureDraw = NewExpression();
            return new StatementExpression.Draw(figureDraw);
        }

        public static StatementExpression ColorStmts()
        {
            if(CurrentToken.StringForm == "blue")
            {
                return new StatementExpression.SetColor(Color.Blue);
            }
            else if(CurrentToken.StringForm == "red")
            {
                return new StatementExpression.SetColor(Color.Red);
            }
            else if(CurrentToken.StringForm == "yellow")
            {
                return new StatementExpression.SetColor(Color.Yellow);
            }
            else if(CurrentToken.StringForm == "green")
            {
                return new StatementExpression.SetColor(Color.Green);
            }
            else if(CurrentToken.StringForm == "cyan")
            {
                return new StatementExpression.SetColor(Color.Cyan);
            }
            else if(CurrentToken.StringForm == "magenta")
            {
                return new StatementExpression.SetColor(Color.Magenta);
            }
            else if(CurrentToken.StringForm == "white")
            {
                return new StatementExpression.SetColor(Color.White);
            }
            else if(CurrentToken.StringForm == "gray")
            {
                return new StatementExpression.SetColor(Color.Gray);
            }
            else if(CurrentToken.StringForm == "black")
            {
                return new StatementExpression.SetColor(Color.Black);
            }
            else throw new DefaultError($"! ERROR : the name {CurrentToken.StringForm} is not a valid color");
        }

        public static StatementExpression RandomPointStmts()
        {
            if(MatchToken(TokenType.ID))
            {
                string name = CurrentToken.StringForm ;
                Next();
                StatementExpression.VariableDeclaration.DeclaredVariables.Add(name);
                return StatementExpression.RandomPoint.NewPoint(name) ;
            }
            else throw new InvalidID(CurrentToken.StringForm, GetLine);
        }

        public static StatementExpression RandomCircleStmts()
        {
            if(MatchToken(TokenType.ID))
            {
                string name = CurrentToken.StringForm ;
                Next();
                StatementExpression.VariableDeclaration.DeclaredVariables.Add(name);
                return new StatementExpression.RandomCircle(name);
            }
            else throw new InvalidID(CurrentToken.StringForm, GetLine);
        }

        public static StatementExpression RandomLineStmts()
        {
            string lineType = CurrentToken.StringForm;
            Next();
            if(MatchToken(TokenType.ID))
            {
                string name = CurrentToken.StringForm ;
                Next();
                StatementExpression.VariableDeclaration.DeclaredVariables.Add(name);
                return new StatementExpression.RandomLine(name , lineType);
            }
            else throw new InvalidID(CurrentToken.StringForm, GetLine);
        }
        
        public static StatementExpression FunctionDeclarationStmst()
        {

            List<string> parameters = new();
            Context functionContext = new Context();
            string functionName = Previous().StringForm;
            if(Lexer.KeyWordsList.Contains(functionName))
            {
                throw new InvalidID(functionName, GetLine);
            }
            functionContext.Variables.Add(functionName , new Expression.ID(functionName , true));
            Next();
            while(!EndOfFile() && ! MatchToken(TokenType.RIGHT_PARENTHESIS))
            {
                if(MatchToken(TokenType.ID))
                {
                    if(parameters.Contains(CurrentToken.StringForm))
                    {
                        throw new DuplicateArgument(CurrentToken.StringForm , GetLine);
                    }
                    else
                    {
                        parameters.Add(CurrentToken.StringForm) ;
                        functionContext.Variables.Add(CurrentToken.StringForm , new Expression.ID(CurrentToken.StringForm , false));//Guardar para identificar como atom
                    }
                }
                else throw new UnexpectedToken(CurrentToken.StringForm);
                Next();
                if(!EndOfFile() && ! MatchToken(TokenType.COMMA))
                {
                    break ;
                }
                else if(!EndOfFile() && MatchToken(TokenType.ID))
                {
                    throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function declaration" , "," , GetLine);
                }
                else Next();
            }
            if(!EndOfFile() && MatchToken(TokenType.RIGHT_PARENTHESIS))
            {
                Next();
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function declaration" , ")" , GetLine);
            if(MatchToken(TokenType.EQUAL))
            {
                Next();
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "function declaration" , "=" , GetLine);
            
            Contexts.Push(functionContext) ;
            Expression body = NewExpression() ;
            StatementExpression.FunctionDeclaration.DeclaredFunctions.Add(functionName);
            //Guardar función cuando se evalúe
            //Añadir contexto y quitar el nombre de la función recursiva como variable
            Contexts.Pop();
            functionContext.Variables.Remove(functionName);
            return new StatementExpression.FunctionDeclaration(functionName , parameters , body , functionContext);
        
        }

        public static StatementExpression VariableDeclarationStmts()
        {
            string variableName = Previous().StringForm;
            if(Lexer.KeyWordsList.Contains(variableName))
            {
                throw new InvalidID(variableName , GetLine);
            }
            Next();
            Expression expr = NewExpression();
            StatementExpression.VariableDeclaration.DeclaredVariables.Add(variableName);
            return new StatementExpression.VariableDeclaration(variableName , expr);
        }

        public static StatementExpression VariableSequenceDeclarationStmts()
        {

            List<string> variablesNames = new();
            variablesNames.Add(Previous().StringForm);
            Next();
            while(!EndOfFile())
            {
                if(MatchToken(TokenType.ID) || MatchToken(TokenType.SKIP))
                {
                    variablesNames.Add(CurrentToken.StringForm);
                    Next();
                }
                else throw new InvalidID(CurrentToken.StringForm , GetLine);
                if(MatchToken(TokenType.COMMA))
                {
                    Next();
                }
                else if(MatchToken(TokenType.EQUAL))
                {
                    break ;
                }
                else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "variable declaration" , "," , GetLine);
            }
            if(!EndOfFile())
            {
                Next();
                Expression sequence = NewExpression();
                if(sequence is Sequence)
                {
                    return new StatementExpression.VariableSequenceDeclaration(variablesNames , (Sequence)sequence );
                }
                else throw new DefaultError($"! ERROR: the value of multiply declaration must be a sequence expression (line : {GetLine})");
            }
            else throw new SyntaxErrors(SyntaxErrorType.MissingToken , "variable declaration" , "=" , GetLine);
        }

        public static StatementExpression ImportStmts()
        {
            if(MatchToken(TokenType.STRING))
            {
                return new StatementExpression.Import(CurrentToken.StringForm.Substring( 1 , CurrentToken.StringForm.Length - 2));
            }
            else throw new DefaultError($"!ERROR: Invalid file name (line : {GetLine})");
        }

        #endregion
    
    }
}