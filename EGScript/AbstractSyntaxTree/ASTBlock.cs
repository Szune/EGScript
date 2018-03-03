using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTBlock : ASTStatementBase
    {
        public List<ASTStatementBase> Statements { get; }
        public ASTBlock(List<ASTStatementBase> block)
        {
            Statements = block;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
