using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTReturn : ASTStatementBase
    {
        public ASTExpressionBase ReturnExpression { get; }

        public ASTReturn(ASTExpressionBase returnExpression)
        {
            ReturnExpression = returnExpression;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
