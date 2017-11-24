using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTTableElement : ASTExpressionBase
    {

        public string StringKey { get; }
        public int IntKey { get; }
        public ASTExpressionBase Value { get; }
        public override ExpressionType Type { get; }

        public ASTTableElement(string key, ASTExpressionBase value)
        {
            StringKey = key;
            Value = value;
            Type = ExpressionType.STRING;
        }

        public ASTTableElement(int key, ASTExpressionBase value)
        {
            IntKey = key;
            Value = value;
            Type = ExpressionType.NUMBER;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
