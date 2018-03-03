using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTIdentifier : ASTExpressionBase
    {
        public string Name { get; }
        public override ExpressionType Type => ExpressionType.IDENTIFIER;

        public ASTIdentifier(string name)
        {
            Name = name;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
