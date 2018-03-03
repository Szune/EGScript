using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTNew : ASTExpressionBase
    {
        public string Name { get; }
        public List<ASTExpressionBase> Arguments { get; }

        public ASTNew(string name, List<ASTExpressionBase> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
