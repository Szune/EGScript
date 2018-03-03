using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTVariableDefinition : ASTMemberDefinition
    {
        public ASTVariableDefinition(string name) : base(name)
        {

        }

        public override void Accept(IVisitor visitor, Class _class)
        {
            visitor.Visit(this, _class);
        }
    }
}
