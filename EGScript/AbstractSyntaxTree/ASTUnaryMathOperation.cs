using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTUnaryMathOperation : ASTExpressionBase
    {
        public enum OperationType
        {
            MINUS,
            NOT,
            INCREMENT,
            DECREMENT
        }

        public OperationType MathOperationType { get; }
        public ASTExpressionBase Expression { get; }

        public ASTUnaryMathOperation(OperationType type, ASTExpressionBase expression)
        {
            MathOperationType = type;
            Expression = expression;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
