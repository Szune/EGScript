using System.Collections.Generic;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTMemberFunction : ASTFunctionBase
    {
        public string BaseClass { get; }

        public ASTMemberFunction(string name, string baseClass, List<string> arguments, ASTStatementBase body)
        {
            Name = name;
            BaseClass = baseClass;
            Arguments = arguments;
            Body = body;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
