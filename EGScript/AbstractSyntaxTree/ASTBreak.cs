using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTBreak : ASTStatementBase
    {

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
