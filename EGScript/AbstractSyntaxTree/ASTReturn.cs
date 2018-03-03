using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTReturn : ASTStatementBase
    {
        public ASTExpressionBase ReturnExpression { get; }

        public ASTReturn(ASTExpressionBase returnExpression)
        {
            ReturnExpression = returnExpression;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
