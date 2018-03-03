using System.Collections.Generic;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public abstract class ASTFunctionBase : IVisitable
    {
        public string Name { get; protected set; }
        public List<string> Arguments { get; protected set; }
        public ASTStatementBase Body { get; protected set; }

        public virtual void Accept(IVisitor visitor)
        {
            throw new CompilerException($"Invalid visit at base function ASTFunctionBase with name '{Name}'. ");
        }
    }
}
