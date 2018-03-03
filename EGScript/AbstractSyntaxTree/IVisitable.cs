using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
