using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTNumber : ASTExpressionBase
    {
        public override ExpressionType Type => ExpressionType.NUMBER;
        public double Value { get; }

        public ASTNumber(double val)
        {
            Value = val;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
