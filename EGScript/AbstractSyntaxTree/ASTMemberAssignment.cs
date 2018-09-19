using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTMemberAssignment : ASTAssignment
    {
        public ASTMemberAssignment(AssignmentType opType, string variable, ASTExpressionBase expression) : base(opType, variable, expression)
        {
        }

        public override ExpressionType Type => ExpressionType.ASSIGNMENT;

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
