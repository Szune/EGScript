using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTWhile : ASTStatementBase
    {
        public ASTExpressionBase Condition { get; }
        public ASTStatementBase Body { get; }

        public ASTWhile(ASTExpressionBase condition, ASTStatementBase body)
        {
            Condition = condition;
            Body = body;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
