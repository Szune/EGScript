using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTFunctionDefinition : ASTMemberDefinition
    {
        public List<string> Arguments { get; }
        public ASTFunctionDefinition(string name, List<string> arguments) : base(name)
        {
            Arguments = arguments;
        }

        public override void Accept(IVisitor visitor, Class _class)
        {
            visitor.Visit(this, _class);
        }
    }
}
