using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
