using System.Collections.Generic;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTClassDefinition : IVisitable
    {
        public string Name { get; }
        public string Base { get; }
        public List<ASTMemberDefinition> MemberDefinitions { get; }

        public ASTClassDefinition(string name, List<ASTMemberDefinition> memberDefinitions)
        {
            Name = name;
            MemberDefinitions = memberDefinitions;
            Base = string.Empty;
        }

        public ASTClassDefinition(string name, string baseClass, List<ASTMemberDefinition> memberDefinitions) : this(name, memberDefinitions)
        {
            Base = baseClass;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
