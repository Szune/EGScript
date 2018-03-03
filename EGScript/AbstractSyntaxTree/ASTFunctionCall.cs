using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public abstract class ASTFunctionCall : ASTExpressionBase
    {
        public string Name { get; }
        public List<ASTExpressionBase> Arguments { get; }
        public abstract override void Accept(IVisitor visitor, Function function);
        
        public ASTFunctionCall(string name, List<ASTExpressionBase> arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}
