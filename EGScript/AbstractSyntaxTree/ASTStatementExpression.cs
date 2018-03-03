using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTStatementExpression : ASTStatementBase
    {
        public ASTExpressionBase Expression { get; }

        public ASTStatementExpression(ASTExpressionBase expression)
        {
            Expression = expression;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
