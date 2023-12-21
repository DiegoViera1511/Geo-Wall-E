using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;

namespace Interpreter
{
    public interface IVisitor
    {
        object Visit(Expression.Atom expr) ;
        object Visit(Expression.ID idExpr);
        object Visit(Expression.Let_In letInExpr);
        object Visit(Expression.Conditional conditionalExpr);
        object Visit(Expression.FunctionCall functionCallExpr);
        object? Visit(StatementExpression.FunctionDeclaration functionDeclaration);
        object? Visit(StatementExpression.VariableDeclaration variableDeclaration);
        object? Visit(StatementExpression.RandomPoint point);
        object Visit(Expression.PointDeclaration point);
        object? Visit(StatementExpression.RandomLine line);
        object Visit(Expression.LineDeclaration line);
        object? Visit(StatementExpression.RandomCircle circle);
        object Visit(Expression.CircleDeclaration circle);
        object Visit(Expression.ArcDeclaration arc);
        object? Visit(StatementExpression.Restore restore);
        object? Visit (StatementExpression.SetColor setColor);
        object Visit(Expression.MeasureExpression measure);
        object? Visit(StatementExpression.Draw draw);
        object Visit(Expression.Atom.Undefined undefined);
        object Visit(ValuesSecquence valuesSecquence);
        object Visit(InfinitySequence infinitySequence);
        object Visit(RangeSequence rangeSequence);
        object? Visit(StatementExpression.VariableSequenceDeclaration variableSequenceDeclaration);
        object Visit(Addition addition);
        object Visit(Subtraction subtraction);
        object Visit(Multiplication multiplication);
        object Visit(Division division);
        object Visit(Module module);
        object Visit(Power power);
        object Visit(LogicAND logicAND);
        object Visit(LogicOR logicOR);
        object Visit(Equality equality);
        object Visit(NotEquality notEquality);
        object Visit(ComparisonGREATER comparisonGREATER);
        object Visit(ComparisonLESS comparisonLESS);
        object Visit(ComparisonGREATER_EQUAL comparisonGREATER_EQUAL);
        object Visit(ComparisonLESS_EQUAL comparisonLESS_EQUAL);
        object Visit(Negation negation);
        object Visit(Negative negative);
        object Visit(Expression.FigureIntersect figureIntersect);
    }

}