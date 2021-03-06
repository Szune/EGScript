﻿using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTString : ASTExpressionBase
    {
        public override ExpressionType Type => ExpressionType.STRING;
        public string Text { get; }
        public ASTString(string text)
        {
            Text = text;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }


    }
}
