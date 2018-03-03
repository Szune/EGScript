using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTBinaryMathOperation : ASTExpressionBase
    {
        public enum OperationType
        {
            /// <summary>
            /// +
            /// </summary>
            PLUS,
            /// <summary>
            /// -
            /// </summary>
            MINUS,
            /// <summary>
            /// *
            /// </summary>
            TIMES,
            /// <summary>
            /// /
            /// </summary>
            DIVIDE
        }

        public OperationType MathOperationType { get; }
        public ASTExpressionBase Left { get; }
        public ASTExpressionBase Right { get; }

        public ASTBinaryMathOperation(OperationType type, ASTExpressionBase left, ASTExpressionBase right)
        {
            MathOperationType = type;
            Left = left;
            Right = right;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
