using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTFunctionDefinition : ASTMemberDefinition
    {
        public List<string> Arguments { get; }
        public ASTStatementBase Body { get; }
        public ASTFunctionDefinition(string name, List<string> arguments, ASTStatementBase body) : base(name)
        {
            Arguments = arguments;
            Body = body;
        }

        public override void Accept(IVisitor visitor, Class _class)
        {
            visitor.Visit(this, _class);
        }
    }
}
