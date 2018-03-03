using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public abstract class ASTStatementBase
    {
        public abstract void Accept(IVisitor visitor, Function function);
    }
}
