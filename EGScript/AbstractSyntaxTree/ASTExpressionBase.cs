using EGScript.Objects;
using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTExpressionBase
    {
        public virtual ExpressionType Type => ExpressionType.NONE;
        public List<ASTExpressionBase> Expressions { get; }

        public ASTExpressionBase()
        {
        }

        public ASTExpressionBase(List<ASTExpressionBase> expressions) : this()
        {
            Expressions = expressions;
        }

        public virtual void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
