using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTGlobalVariableAssignment : ASTAssignment
    {
        public ASTGlobalVariableAssignment(string variable, ASTExpressionBase expression) : base(AssignmentType.ASSIGNMENT, variable, expression)
        {
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            throw new CompilerException($"Invalid visit at global variable assignment {nameof(ASTGlobalVariableAssignment)} of variable '{Variable}'.");
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
