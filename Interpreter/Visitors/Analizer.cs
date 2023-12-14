namespace Interpreter
{
    public class Analizer : IVisitor
    {
        public object Visit(Expression.Atom expr)
        {
            return expr.ExpType ;
        }

        public object Visit(Expression.ID idExpr)
        {
            if(Parser.CurrentContext.FunctionsArgumentsValue.ContainsKey(idExpr.Name))
            {
                return Parser.GetObjectType(Parser.CurrentContext.FunctionsArgumentsValue[idExpr.Name]);
            }
            if(Parser.CurrentContext.Variables.ContainsKey(idExpr.Name))
            {
                if(Parser.CurrentContext.Variables[idExpr.Name] is Expression.ID)
                {
                    return ExpressionType.Inference ;
                }
                return Parser.CurrentContext.Variables[idExpr.Name].Accept(this);
            }
            else if(Parser.CurrentContext.FiguresVariables.ContainsKey(idExpr.Name))
            {
                return ExpressionType.Figure;
            }
            else
            {
               throw new SyntaxErrors(SyntaxErrorType.DoNotExistID , idExpr.Name , idExpr.ExpressionLine);
            }
        }

        public object Visit(Expression.Let_In letInExpr)
        {
            return letInExpr.Body.Accept(this);
        }

        public object Visit(Expression.Conditional conditionalExpr)
        {
            object checkIF = conditionalExpr.IfExpression.Accept(this);
            object checkThen = conditionalExpr.ThenExpression.Accept(this);
            object checkElse = conditionalExpr.ElseExpression.Accept(this);
            return ExpressionType.Inference ;
        }

        public object Visit(Expression.FunctionCall functionCallExpr)
        {
            string functionName = functionCallExpr.FunctionName ;
            foreach(Expression expr in functionCallExpr.ParametersValue)
            {
                object check = expr.Accept(this);
            }
            if( Function.FunctionStore.ContainsKey( functionName ) )
            {
                if(Function.FunctionStore[functionName].Parameters.Count != functionCallExpr.ParametersValue.Count)
                {
                    throw new ArgumentCountError(functionName , Function.FunctionStore[functionName].Parameters.Count , functionCallExpr.ParametersValue.Count );
                }

                if(Parser.AnalizeFunctionsStack.Contains(functionName))
                {
                    return ExpressionType.Inference ;
                }

                Context functionContext = new Context();
                for(int i = 0 ; i < functionCallExpr.ParametersValue.Count ; i++)
                {
                    functionContext.FunctionsArgumentsValue.Add(Function.FunctionStore[functionName].Parameters[i] , functionCallExpr.ParametersValue[i].Accept(this));
                }
                Parser.Contexts.Push(functionContext);
                Parser.AnalizeFunctionsStack.Push(functionName);
                try
                {
                    object exprType = Function.FunctionStore[functionName].Body.Accept(this) ;
                    Parser.Contexts.Pop();
                    Parser.AnalizeFunctionsStack.Pop();
                    return exprType ;
                }
                catch (IncorrectBinaryExpression binaryError)
                {
                    Parser.Contexts.Pop();
                    Parser.AnalizeFunctionsStack.Pop();
                    if( binaryError.operatorProblem == "==" || binaryError.operatorProblem == "!=" )
                    {
                        throw binaryError ;
                    }
                    else
                    {
                        throw new ArgumentTypeError(binaryError.expectedType.ToString() , binaryError.badType.ToString() , functionCallExpr.ExpressionLine , functionName );
                    }
                }
            }
            else return ExpressionType.Inference ;
        }

        public object? Visit(StatementExpression.FunctionDeclaration functionDeclaration)
        {
            if(Function.FunctionStore.ContainsKey(functionDeclaration.FunctionName))
            {
                throw new DefaultError($"! ERROR: function name {functionDeclaration.FunctionName} already exist in the current context(line : {functionDeclaration.ExpressionLine}).");
            }
            Parser.Contexts.Push(functionDeclaration.FunctionContext);
            object checkBody = functionDeclaration.Body.Accept(this);
            Parser.Contexts.Pop();
            return ExpressionType.Statement ;
        }

        public object? Visit(StatementExpression.VariableDeclaration variableDeclaration)
        {
            object check = variableDeclaration.Value.Accept(this);
            return ExpressionType.Statement ;
        }

        public object? Visit(StatementExpression.RandomPoint point)
        {
            return ExpressionType.Statement ;
        }

        public object Visit(Expression.PointDeclaration point)
        {
            return ExpressionType.Figure ;
        }

        public object? Visit(StatementExpression.RandomLine line)
        {
            return ExpressionType.Statement ;
        }

        public object Visit(Expression.LineDeclaration line)
        {
            return ExpressionType.Figure ;
        }

        public object? Visit(StatementExpression.RandomCircle circle)
        {
            return ExpressionType.Figure ; 
        }

        public object Visit(Expression.CircleDeclaration circle)
        {
            return ExpressionType.Figure ;
        }

        public object Visit(Expression.ArcDeclaration arc)
        {
            return ExpressionType.Figure ;
        }

        public object? Visit(StatementExpression.Restore restore)
        {
            return ExpressionType.Statement ;
        }

        public object? Visit(StatementExpression.SetColor setColor)
        {
            return ExpressionType.Statement ;
        }

        public object Visit(Expression.MeasureExpression measure)
        {
            return ExpressionType.Number ;
        }

        public object? Visit(StatementExpression.Draw draw)
        {

            ExpressionType type = (ExpressionType)draw.FigureDraw.Accept(this);

            if(type.Equals(ExpressionType.Inference)) 
            {
                return ExpressionType.Statement ;
            }
            else if(type.Equals(ExpressionType.Sequence))
            {
                if(draw.FigureDraw is ValuesSecquence)
                {
                    ValuesSecquence sec = (ValuesSecquence)draw.FigureDraw ;
                    object checktype = sec.Accept(this);
                    if(sec.SequenceValues.Count != 0)
                    {
                        object elementsType = sec.SequenceValues[0].Accept(this);
                        if(elementsType is not ExpressionType.Figure)
                        {
                            throw new DefaultError($"! ERROR: Draw function receives Figures not {elementsType} (line : {draw.ExpressionLine})");
                        }
                    }
                }
                else throw new DefaultError($"!ERROR: Draw function receives Figures or Figures sequences not (line : {draw.ExpressionLine})");
            }
            else if (!type.Equals(ExpressionType.Figure))
            {
                throw new DefaultError($"!ERROR: Draw function receives Figures or Figures sequences (line : {draw.ExpressionLine})");
            }
            return ExpressionType.Statement ;
        }

        public object Visit(Expression.Atom.Undefined undefined)
        {
            return ExpressionType.Undefined ;
        }

        public object Visit(ValuesSecquence valuesSecquence)
        {
            if(valuesSecquence.SequenceValues.Count != 0)
            {
                ExpressionType type = (ExpressionType)valuesSecquence.SequenceValues[0].Accept(this);
                foreach(Expression expr in valuesSecquence.SequenceValues)
                {
                    ExpressionType exprType = (ExpressionType)expr.Accept(this);
                    if(!type.Equals(exprType))
                    {
                        throw new DefaultError($"! ERROR: Sequence can only contain one type of object (line : {valuesSecquence.ExpressionLine})");
                    }
                }
            }
            return ExpressionType.Sequence ;
        }

        public object Visit(InfinitySequence infinitySequence)
        {
            return ExpressionType.Sequence ;
        }

        public object Visit(RangeSequence rangeSequence)
        {
            return ExpressionType.Sequence ;
        }

        public object? Visit(StatementExpression.VariableSequenceDeclaration variableSequenceDeclaration)
        {
            object check = variableSequenceDeclaration.ValuesSequence.Accept(this);
            return null ;
       }

        public object Visit(Addition addition)
        {
            ExpressionType leftType = (ExpressionType)addition.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)addition.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Number ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("+"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , addition.ExpressionLine);
            }
        }

        public object Visit(Subtraction subtraction)
        {
            ExpressionType leftType = (ExpressionType)subtraction.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)subtraction.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Number ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("-"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , subtraction.ExpressionLine);
            }
        }

        public object Visit(Multiplication multiplication)
        {
            ExpressionType leftType = (ExpressionType)multiplication.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)multiplication.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Number ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("*"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , multiplication.ExpressionLine);
            }
        }

        public object Visit(Division division)
        {
            ExpressionType leftType = (ExpressionType)division.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)division.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Number ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("/"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , division.ExpressionLine);
            }
        }

        public object Visit(Module module)
        {
            ExpressionType leftType = (ExpressionType)module.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)module.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Number ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("%"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , module.ExpressionLine);
            }
        }

        public object Visit(Power power)
        {
            ExpressionType leftType = (ExpressionType)power.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)power.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Number ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("^"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , power.ExpressionLine);
            }
        }

        public object Visit(LogicAND logicAND)
        {
            ExpressionType leftType = (ExpressionType)logicAND.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)logicAND.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Boolean ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Boolean ;
            }
            if(leftType == ExpressionType.Boolean && rightType == ExpressionType.Boolean)
            {
                return ExpressionType.Boolean ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Boolean)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("&"  , ExpressionType.Boolean , badType , leftType.ToString() , rightType.ToString() , logicAND.ExpressionLine);
            }
        }

        public object Visit(LogicOR logicOR)
        {
            ExpressionType leftType = (ExpressionType)logicOR.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)logicOR.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Boolean ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Boolean ;
            }
            if(leftType == ExpressionType.Boolean && rightType == ExpressionType.Boolean)
            {
                return ExpressionType.Boolean ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Boolean)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("|"  , ExpressionType.Boolean , badType , leftType.ToString() , rightType.ToString() , logicOR.ExpressionLine);
            }
        }

        public object Visit(Equality equality)
        {
            ExpressionType leftType = (ExpressionType)equality.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)equality.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = rightType ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = leftType ;
            }
            if(leftType.Equals(rightType))
            {
                return leftType ;
            }
            else throw new IncorrectBinaryExpression("==" , ExpressionType.Inference  , ExpressionType.Inference , leftType.ToString() , rightType.ToString() , equality.ExpressionLine);
        }

        public object Visit(NotEquality notEquality)
        {
            ExpressionType leftType = (ExpressionType)notEquality.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)notEquality.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = rightType ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = leftType ;
            }
            if(leftType.Equals(rightType))
            {
                return leftType ;
            }
            else throw new IncorrectBinaryExpression("!=" , ExpressionType.Inference  , ExpressionType.Inference , leftType.ToString() , rightType.ToString() , notEquality.ExpressionLine);
        }

        public object Visit(ComparisonGREATER comparisonGREATER)
        {
            ExpressionType leftType = (ExpressionType)comparisonGREATER.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)comparisonGREATER.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Boolean ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression(">"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , comparisonGREATER.ExpressionLine);
            }
        }

        public object Visit(ComparisonLESS comparisonLESS)
        {
            ExpressionType leftType = (ExpressionType)comparisonLESS.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)comparisonLESS.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Boolean ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("<"  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , comparisonLESS.ExpressionLine);
            }
        }

        public object Visit(ComparisonGREATER_EQUAL comparisonGREATER_EQUAL)
        {
            ExpressionType leftType = (ExpressionType)comparisonGREATER_EQUAL.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)comparisonGREATER_EQUAL.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Boolean ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression(">="  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , comparisonGREATER_EQUAL.ExpressionLine);
            }
        }

        public object Visit(ComparisonLESS_EQUAL comparisonLESS_EQUAL)
        {
            ExpressionType leftType = (ExpressionType)comparisonLESS_EQUAL.Left.Accept(this);
            ExpressionType rightType = (ExpressionType)comparisonLESS_EQUAL.Right.Accept(this);

            if(leftType == ExpressionType.Inference)
            {
                leftType = ExpressionType.Number ;
            }
            if(rightType == ExpressionType.Inference)
            {
                rightType = ExpressionType.Number ;
            }
            if(leftType == ExpressionType.Number && rightType == ExpressionType.Number)
            {
                return ExpressionType.Boolean ;
            }
            else 
            {
                ExpressionType badType ;
                if(leftType != ExpressionType.Number)
                {
                    badType = leftType;
                }
                else badType = rightType ;
                throw new IncorrectBinaryExpression("<="  , ExpressionType.Number , badType , leftType.ToString() , rightType.ToString() , comparisonLESS_EQUAL.ExpressionLine);
            }
        }

        public object Visit(Negation negation)
        {
            ExpressionType type = (ExpressionType)negation.Right.Accept(this);
            if(type == ExpressionType.Boolean)
            {
                return ExpressionType.Boolean ;
            }
            else throw new IncorrectOperator(type.ToString() , ExpressionType.Boolean, "!" , negation.ExpressionLine);
        }

        public object Visit(Negative negative)
        {
            ExpressionType type = (ExpressionType)negative.Right.Accept(this);
            if(type == ExpressionType.Number)
            {
                return ExpressionType.Number ;
            }
            else throw new IncorrectOperator(type.ToString() , ExpressionType.Number , "-" , negative.ExpressionLine);
        }
    }
}