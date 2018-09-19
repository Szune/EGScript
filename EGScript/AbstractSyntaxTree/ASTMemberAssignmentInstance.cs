using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTMemberAssignmentInstance : ASTMemberAssignment
    {
        public string InstanceName { get; }

        public ASTMemberAssignmentInstance(AssignmentType opType, string variable, ASTExpressionBase expression, string instanceName) : base(opType, variable, expression)
        {
            InstanceName = instanceName;
        }

        
        public override ExpressionType Type => ExpressionType.ASSIGNMENT;

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
