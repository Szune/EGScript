using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTMemberAccess : ASTIdentifier
    {
        public string InstanceName { get; }

        public ASTMemberAccess(string memberName, string instanceName) : base(memberName)
        {
            InstanceName = instanceName;
        }

        public override ExpressionType Type => ExpressionType.IDENTIFIER;

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
