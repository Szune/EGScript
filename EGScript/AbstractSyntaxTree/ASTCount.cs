using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTCount : ASTExpressionBase
    {
        public string Identifier { get; }

        public ASTCount(string identifier)
        {
            Identifier = identifier;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
