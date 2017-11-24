using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTStatementExpression : ASTStatementBase
    {
        public ASTExpressionBase Expression { get; }

        public ASTStatementExpression(ASTExpressionBase expression)
        {
            Expression = expression;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
