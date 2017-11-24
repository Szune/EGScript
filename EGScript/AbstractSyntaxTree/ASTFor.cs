using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTFor : ASTStatementBase
    {
        public ASTExpressionBase Initializer { get; }
        public ASTExpressionBase Condition { get; }
        public ASTExpressionBase Incrementer { get; }
        public ASTStatementBase Body { get; }

        public ASTFor(ASTExpressionBase initializer, ASTExpressionBase condition, ASTExpressionBase incrementer, ASTStatementBase body)
        {
            Initializer = initializer;
            Condition = condition;
            Incrementer = incrementer;
            Body = body;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
