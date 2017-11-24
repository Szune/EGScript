using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTIf : ASTStatementBase
    {
        public ASTExpressionBase Condition { get; }
        public ASTStatementBase IfPart { get; }
        public ASTStatementBase ElsePart { get; }

        public ASTIf(ASTExpressionBase condition, ASTStatementBase ifPart)
        {
            Condition = condition;
            IfPart = ifPart;
        }

        public ASTIf(ASTExpressionBase condition, ASTStatementBase ifPart, ASTStatementBase elsePart) : this(condition, ifPart)
        {
            ElsePart = elsePart;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
