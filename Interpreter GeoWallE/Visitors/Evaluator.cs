namespace Interpreter
{
    public class Evaluator : IVisitor
    {
        public object Visit(Expression.Atom expr)
        {
            return expr.Value ;
        }

        public object Visit(Expression.ID idExpr)
        {
            if(Parser.CurrentContext.FunctionsArgumentsValue.ContainsKey(idExpr.Name))
            {
                return Parser.CurrentContext.FunctionsArgumentsValue[idExpr.Name];
            }
            else if(Parser.CurrentContext.Variables.ContainsKey(idExpr.Name))
            {
                return Parser.CurrentContext.Variables[idExpr.Name].Accept(this);
            }
            else if(Parser.CurrentContext.FiguresVariables.ContainsKey(idExpr.Name))
            {
                return Parser.CurrentContext.FiguresVariables[idExpr.Name];
            }
            else
            {
                throw new SyntaxErrors(SyntaxErrorType.DoNotExistID , idExpr.Name , idExpr.ExpressionLine);
            }
        }

        public object Visit(Expression.Let_In letInExpr)
        {
            foreach(StatementExpression stmts in letInExpr.LetInStatements)
            {
                stmts.Accept(this);
            }
            object result = letInExpr.Body.Accept(this);   
            Expression.Let_In.RemoveLocalVariables(letInExpr.LocalVariables);
            return result ;        
        }

        public object Visit(Expression.Conditional conditionalExpr)
        {
            object condition = conditionalExpr.IfExpression.Accept(this);

            if(Parser.GetObjectType(condition) != ExpressionType.Boolean)
            {
                throw new ConditionalErrors(Parser.GetObjectType(condition).ToString() , conditionalExpr.ExpressionLine);
            }

            if((bool)condition)
            {
                return conditionalExpr.ThenExpression.Accept(this);
            }
            return conditionalExpr.ElseExpression.Accept(this);

        }

        public object Visit(Expression.FunctionCall functionCallExpr)
        {
            if(Expression.FunctionCall.FunctionCallStack[functionCallExpr.FunctionName] > 1000)
            {
                throw new DefaultError($"! ERROR: StackOverflow ! function {functionCallExpr.FunctionName} (line : {functionCallExpr.ExpressionLine})");
            }
            string functionName = functionCallExpr.FunctionName ;

            Context functionContext = new Context();

            for(int i = 0 ; i < functionCallExpr.ParametersValue.Count ; i++)
            {
                functionContext.FunctionsArgumentsValue.Add(Function.FunctionStore[functionName].Parameters[i] , functionCallExpr.ParametersValue[i].Accept(this));
            }
            Parser.Contexts.Push(functionContext);
            object result = Function.FunctionStore[functionName].Body.Accept(this);
            Parser.Contexts.Pop();
            return result ;
        }

        public object? Visit(StatementExpression.FunctionDeclaration functionDeclaration)
        {
            //error de numbre de función repetida
            Expression.FunctionCall.FunctionCallStack.Add(functionDeclaration.FunctionName , 0);
            Function.FunctionStore.Add(functionDeclaration.FunctionName , new Function(functionDeclaration.FunctionName , functionDeclaration.Body , functionDeclaration.Parameters));
            return null ;
        }

        public object? Visit(StatementExpression.VariableDeclaration variableDeclaration)
        {
            Parser.CurrentContext.AddVariable(variableDeclaration.VariableName , variableDeclaration.Value);  
            return null ;
        }

        public object? Visit(StatementExpression.RandomPoint point)
        {
            Parser.CurrentContext.AddVariable(point.Name , new Point( point.X , point.Y ));
            return null ;
        }
        
        public object Visit(Expression.PointDeclaration point)
        {
            return new Point((double)point.X.Accept(this) , (double)point.Y.Accept(this));
        }

        public object? Visit(StatementExpression.RandomLine line)
        {
            Random number = new Random();
            Point p1 = new Point(number.Next(100 , 700), number.Next(100 , 700)) ;
            Point p2 = new Point(number.Next(100 , 700) , number.Next(100 , 700)) ;
            if(line.LineType == "line")
            {
                Parser.CurrentContext.AddVariable(line.Name , new Line(p1 , p2));
            }
            else if(line.LineType == "segment")
            {
                Parser.CurrentContext.AddVariable(line.Name , new Segment(p1 , p2));
            }
            else
            {
                Parser.CurrentContext.AddVariable(line.Name , new Ray(p1 , p2));
            }
            return null ;
        }

        public object Visit(Expression.LineDeclaration line)
        {
            if(Parser.CurrentContext.FiguresVariables.ContainsKey(line.P1Name))
            {
                if(Parser.CurrentContext.FiguresVariables[line.P1Name] is Point)
                {
                    Point p1 = (Point)Parser.CurrentContext.FiguresVariables[line.P1Name];
                    if(Parser.CurrentContext.FiguresVariables.ContainsKey(line.P2Name))
                    {
                        if(Parser.CurrentContext.FiguresVariables[line.P2Name] is Point)
                        {
                            Point p2 = (Point)Parser.CurrentContext.FiguresVariables[line.P2Name];
                            if(line.LineType == "line")
                            {
                                return new Line(p1 , p2);
                            }
                            else if (line.LineType == "segment")
                            {
                                return new Segment(p1 , p2);
                            }
                            else return new Ray(p1 , p2);
                        }
                        else throw new DefaultError($"Line receives point, not {Parser.CurrentContext.FiguresVariables[line.P2Name].GetType()}");
                    }
                    else throw new DefaultError($"! ERROR: point {line.P2Name} do not exist in the current context");
                }
                else throw new DefaultError($"Line receives point, not {Parser.CurrentContext.FiguresVariables[line.P2Name].GetType()}");
            }
            else throw new DefaultError($"! ERROR: point {line.P1Name} do not exist in the current context");
        }

        public object Visit(Expression.ArcDeclaration arc)
        {
            if(Parser.CurrentContext.FiguresVariables.ContainsKey(arc.P1Name))
            {
                if(Parser.CurrentContext.FiguresVariables[arc.P1Name] is Point)
                {
                    Point p1 = (Point)Parser.CurrentContext.FiguresVariables[arc.P1Name];
                    if(Parser.CurrentContext.FiguresVariables.ContainsKey(arc.P2Name))
                    {
                        if(Parser.CurrentContext.FiguresVariables[arc.P2Name] is Point)
                        {
                            Point p2 = (Point)Parser.CurrentContext.FiguresVariables[arc.P2Name];
                            if(Parser.CurrentContext.FiguresVariables.ContainsKey(arc.P3Name))
                            {
                                if(Parser.CurrentContext.FiguresVariables[arc.P3Name] is Point)
                                {
                                    Point p3 = (Point)Parser.CurrentContext.FiguresVariables[arc.P3Name];
                                    object m = arc.Measure.Accept(this);
                                    if(m is double)
                                    {
                                        return new Arc( p1 , p2 , p3 , (double)m );
                                    }
                                    else throw new DefaultError($"! ERROR: arc radius receives Number not {Parser.GetObjectType(m)}");
                                }
                                else throw new DefaultError($"Arc receives point, not {arc.P3Name} : {Parser.CurrentContext.FiguresVariables[arc.P3Name].GetType()}");
                            }
                            else throw new DefaultError($"! ERROR: point {arc.P3Name} do not exist in the current context");
                        }
                        else throw new DefaultError($"Arc receives point, not {arc.P2Name} : {Parser.CurrentContext.FiguresVariables[arc.P2Name].GetType()}");
                    }
                    else throw new DefaultError($"! ERROR: point {arc.P2Name} do not exist in the current context");
                }
                else throw new DefaultError($"Arc receives point, not {arc.P1Name} : {Parser.CurrentContext.FiguresVariables[arc.P1Name].GetType()}");
            }
            else throw new DefaultError($"! ERROR: point {arc.P1Name} do not exist in the current context");
        }

        public object? Visit(StatementExpression.RandomCircle circle)
        {
            Random number = new Random();
            Point center = new Point(number.Next(200 , 600) ,number.Next(200 , 600)) ;
            Circle c = new Circle(center , number.Next(100 , 200));
            Parser.CurrentContext.AddVariable(circle.Name , c );
            return null ;

        }

        public object Visit(Expression.CircleDeclaration circle)
        {
            object measure = circle.Measure.Accept(this);
            if(measure is double)
            {
                if(Parser.CurrentContext.FiguresVariables.ContainsKey(circle.CenterName))
                {
                    if(Parser.CurrentContext.FiguresVariables[circle.CenterName] is Point)
                    {
                        return new Circle((Point)Parser.CurrentContext.FiguresVariables[circle.CenterName] , (double)measure) ;
                    }
                    else throw new DefaultError($"Arc receives point, not {circle.CenterName} : {Parser.CurrentContext.FiguresVariables[circle.CenterName].GetType()}");
                }
                else throw new DefaultError($"! ERROR: point {circle.CenterName} do not exist in the current context");
            }
            else throw new DefaultError($"! ERROR: circle radius receives Number not {Parser.GetObjectType(measure)}");
        }

        public object? Visit(StatementExpression.Restore restore)
        {
            Parser.actualColor = Color.Black ;
            return null ;
        }
       
        public object? Visit(StatementExpression.SetColor setColor)
        {
            Parser.actualColor = setColor.Color ;
            return null ;
        }

        public object Visit(Expression.MeasureExpression measure)
        {
            if(Parser.CurrentContext.FiguresVariables.ContainsKey(measure.P1Name))
            {
                Point p1 = (Point)Parser.CurrentContext.FiguresVariables[measure.P1Name];
                if(Parser.CurrentContext.FiguresVariables.ContainsKey(measure.P2Name))
                {
                    Point p2 = (Point)Parser.CurrentContext.FiguresVariables[measure.P2Name];
                    return Math.Sqrt(Math.Pow(p1.X - p2.X , 2) + Math.Pow(p1.Y - p2.Y , 2));
                }
                else throw new DefaultError($"! ERROR: point {measure.P2Name} do not exist in the current context");
            }
            else throw new DefaultError($"! ERROR: point {measure.P1Name} do not exist in the current context");
        }

        public object? Visit(StatementExpression.Draw draw)
        {
            if(draw.FigureDraw is Expression.ID)
            {
                Expression.ID idExpr = (Expression.ID)draw.FigureDraw ;
                object expr = idExpr.Accept(this);
                if(expr is Figure)
                {
                    Parser.Prints.Add((Figure)expr) ;
                }
                else if(expr is ValuesSecquence)
                {
                    ValuesSecquence sec = (ValuesSecquence)expr;
                    foreach(var figure in sec.SequenceValues)
                    {
                        object fig = figure.Accept(this);
                        if(fig is Figure)
                        {
                            Parser.Prints.Add((Figure)fig);
                        }
                        else throw new DefaultError($"! ERROR: Draw function receives Figures not {Parser.GetObjectType(fig)} (line : {draw.ExpressionLine})");
                    }
                }
                else throw new DefaultError($"!ERROR: Draw function receives Figures or Figures sequences not {Parser.GetObjectType(expr)} (line : {draw.ExpressionLine})");
            }
            else if(draw.FigureDraw is Sequence)
            {
                if(draw.FigureDraw is ValuesSecquence)
                {
                    ValuesSecquence sec = (ValuesSecquence)draw.FigureDraw;
                    foreach(var figure in sec.SequenceValues)
                    {
                        object fig = figure.Accept(this);
                        if(fig is Figure)
                        {
                            Parser.Prints.Add((Figure)fig);
                        }
                        else throw new DefaultError($"! ERROR: Draw function receives Figures not {Parser.GetObjectType(fig)} (line : {draw.ExpressionLine})");
                    }
                }
                else throw new DefaultError($"! ERROR: Draw function receives only Figures sequences (line : {draw.ExpressionLine})");
            }
            else throw new DefaultError($"!ERROR: Draw function receives Figures or Figures sequences (line : {draw.ExpressionLine})");
            return null ;
        }

        public object Visit(Expression.Atom.Undefined undefined)
        {
            return undefined.ExpType ;
        }

        public object Visit(ValuesSecquence valuesSecquence)
        {
            return valuesSecquence.SequenceValues ;
        }

        public object Visit(InfinitySequence infinitySequence)
        {
            return infinitySequence ;
        }

        public object Visit(RangeSequence rangeSequence)
        {
            return rangeSequence ;
        }

        public object? Visit(StatementExpression.VariableSequenceDeclaration variableSequenceDeclaration)
        {
            IEnumerable<object> values ;
            object ValuesEnumerable = variableSequenceDeclaration.ValuesSequence.Accept(this) ;
            values = (IEnumerable<object>)ValuesEnumerable ;
            IEnumerator<object> enumerator = values.GetEnumerator() ;

            if(variableSequenceDeclaration.ValuesSequence is ValuesSecquence)
            {
                for(int i = 0 ; i < variableSequenceDeclaration.VariablesNames.Count() ; i++)
                {
                    if(variableSequenceDeclaration.VariablesNames[i] == "_" && enumerator.MoveNext())
                    {
                        continue ;
                    }
                    if(i >= variableSequenceDeclaration.VariablesNames.Count() - 1)
                    {
                        List<Expression> restExpr = new();
                        while(enumerator.MoveNext())
                        {
                            restExpr.Add((Expression)enumerator.Current);
                        }
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] , new ValuesSecquence(restExpr));
                    }
                    else if(enumerator.MoveNext())
                    {
                        Expression value = (Expression)enumerator.Current ;
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] , value );
                    }
                    else
                    {
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] ,  new Expression.Atom.Undefined() );
                    }
                }
            }
            else if( variableSequenceDeclaration.ValuesSequence is InfinitySequence )
            {
                for(int i = 0 ; i < variableSequenceDeclaration.VariablesNames.Count() ; i++)
                {
                    if(variableSequenceDeclaration.VariablesNames[i] == "_" && enumerator.MoveNext())
                    {
                        continue ;
                    }
                    if(i >= variableSequenceDeclaration.VariablesNames.Count() - 1 && variableSequenceDeclaration.VariablesNames[i] != "_" && enumerator.MoveNext())
                    {
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] , new InfinitySequence((double)enumerator.Current));
                    }
                    else if(enumerator.MoveNext())
                    {
                        double value = (double)enumerator.Current ;
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] , new Expression.Atom.Number(value));
                    }
                    else
                    {
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] ,  new Expression.Atom.Undefined() );
                    }
                }
            }
            else if( variableSequenceDeclaration.ValuesSequence is RangeSequence )
            {
                for(int i = 0 ; i < variableSequenceDeclaration.VariablesNames.Count() ; i++)
                {
                    if(variableSequenceDeclaration.VariablesNames[i] == "_" && enumerator.MoveNext())
                    {
                        continue ;
                    }
                    if(i >= variableSequenceDeclaration.VariablesNames.Count() - 1)
                    {
                        if(enumerator.MoveNext())
                        {
                            RangeSequence rangeSec = (RangeSequence)variableSequenceDeclaration.ValuesSequence ;
                            Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] , new RangeSequence((double)enumerator.Current , rangeSec.Max));
                        }
                        else
                        {
                            Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] , new ValuesSecquence(new List<Expression>()));
                        }
                    }
                    else if(enumerator.MoveNext())
                    {
                        double value = (double)enumerator.Current ;
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] , new Expression.Atom.Number( value) );
                    }
                    else
                    {
                        Parser.CurrentContext.AddVariable(variableSequenceDeclaration.VariablesNames[i] ,  new Expression.Atom.Undefined() );
                    }
                }
            }
            return null ;
        }

        public object Visit(Addition addition)
        {
            object num1 = addition.Left.Accept(this);
            object num2 = addition.Right.Accept(this);
            return (double)num1 + (double)num2 ;
        }

        public object Visit(Subtraction subtraction)
        {
            object num1 = subtraction.Left.Accept(this);
            object num2 = subtraction.Right.Accept(this);
            return (double)num1 - (double)num2 ;
        }

        public object Visit(Multiplication multiplication)
        {
            object num1 = multiplication.Left.Accept(this);
            object num2 = multiplication.Right.Accept(this);
            return (double)num1 * (double)num2 ;
        }

        public object Visit(Division division)
        {
            object num1 = division.Left.Accept(this);
            object num2 = division.Right.Accept(this);
            return (double)num1 / (double)num2 ;
        }

        public object Visit(Module module)
        {
            object num1 = module.Left.Accept(this);
            object num2 = module.Right.Accept(this);
            return (double)num1 % (double)num2 ;
        }

        public object Visit(Power power)
        {
            object num1 = power.Left.Accept(this);
            object num2 = power.Right.Accept(this);
            return Math.Pow((double)num1 , (double)num2);
        }

        public object Visit(LogicAND logicAND)
        {
            object term1 = logicAND.Left.Accept(this);
            object term2 = logicAND.Right.Accept(this);
            return (bool)term1 && (bool)term2 ;
        }

        public object Visit(LogicOR logicOR)
        {
            object term1 = logicOR.Left.Accept(this);
            object term2 = logicOR.Right.Accept(this);
            return (bool)term1 || (bool)term2 ;
        }

        public object Visit(Equality equality)
        {
            object term1 = equality.Left.Accept(this);
            object term2 = equality.Right.Accept(this);
            return term1.Equals(term2);
        }

        public object Visit(NotEquality notEquality)
        {
            object term1 = notEquality.Left.Accept(this);
            object term2 = notEquality.Right.Accept(this);
            return ! term1.Equals(term2);
        }

        public object Visit(ComparisonGREATER comparisonGREATER)
        {
            object num1 = comparisonGREATER.Left.Accept(this);
            object num2 = comparisonGREATER.Right.Accept(this);
            return (double)num1 > (double)num2 ;
        }

        public object Visit(ComparisonLESS comparisonLESS)
        {
            object num1 = comparisonLESS.Left.Accept(this);
            object num2 = comparisonLESS.Right.Accept(this);
            return (double)num1 < (double)num2 ;
        }

        public object Visit(ComparisonGREATER_EQUAL comparisonGREATER_EQUAL)
        {
            object num1 = comparisonGREATER_EQUAL.Left.Accept(this);
            object num2 = comparisonGREATER_EQUAL.Right.Accept(this);
            return (double)num1 >= (double)num2 ;
        }

        public object Visit(ComparisonLESS_EQUAL comparisonLESS_EQUAL)
        {
            object num1 = comparisonLESS_EQUAL.Left.Accept(this);
            object num2 = comparisonLESS_EQUAL.Right.Accept(this);
            return (double)num1 <= (double)num2 ;
        }

        public object Visit(Negation negation)
        {
            object term = negation.Right.Accept(this);
            return ! (bool)term ;
        }

        public object Visit(Negative negative)
        {
            object num = negative.Right.Accept(this);
            return -1 * (double)num ;
        }
    }
}