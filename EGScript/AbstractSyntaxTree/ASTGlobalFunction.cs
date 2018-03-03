using System.Collections.Generic;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTGlobalFunction : ASTFunctionBase
    {
        public ASTGlobalFunction(string name, List<string> arguments, ASTStatementBase body)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
