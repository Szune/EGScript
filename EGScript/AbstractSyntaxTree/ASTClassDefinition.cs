using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
