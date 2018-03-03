using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTMemberDefinition
    {
        public string Name { get; }

        public ASTMemberDefinition(string name)
        {
            Name = name;
        }

        public virtual void Accept(IVisitor visitor, Class _class)
        {
            throw new CompilerException($"Invalid visit at class member definition ASTMemberDefinition with name '{Name}'. ");
        }
    }
}
