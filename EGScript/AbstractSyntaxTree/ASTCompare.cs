using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTCompare : ASTExpressionBase
    {
        public enum OperationType
        {
            OR,
            AND,
            EQUALS_EQUALS,
            NOT_EQUALS,
            LESS_THAN,
            GREATER_THAN,
            LESS_THAN_EQUALS,
            GREATER_THAN_EQUALS
        }

        public override ExpressionType Type => ExpressionType.COMPARE;
        public OperationType ComparisonType { get; }
        public ASTExpressionBase Left { get; }
        public ASTExpressionBase Right { get; }

        public ASTCompare(OperationType type, ASTExpressionBase left, ASTExpressionBase right)
        {
            ComparisonType = type;
            Left = left;
            Right = right;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
