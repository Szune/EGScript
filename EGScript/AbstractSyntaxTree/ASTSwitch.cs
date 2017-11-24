using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTSwitch : ASTStatementBase
    {
        public ASTExpressionBase Expression { get; }
        public List<(ASTExpressionBase expression, ASTStatementBase statement)> Cases { get; }

        public ASTSwitch(ASTExpressionBase expression)
        {
            Expression = expression;
            Cases = new List<(ASTExpressionBase expression, ASTStatementBase statement)>();
        }


        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }

        public void AddCase(ASTExpressionBase identifier, ASTStatementBase statement)
        {
            Cases.Add((identifier, statement));
        }
    }
}
