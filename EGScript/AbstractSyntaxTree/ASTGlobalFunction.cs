using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
