using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTGlobalFunctionCall : ASTFunctionCall
    {
        public ASTGlobalFunctionCall(string name, List<ASTExpressionBase> arguments) : base(name, arguments)
        {
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
