using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTMemberFunctionCall : ASTFunctionCall
    {
        public string Base { get; }

        public ASTMemberFunctionCall(string name, string baseClass, List<ASTExpressionBase> arguments) : base(name, arguments)
        {
            Base = baseClass;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
