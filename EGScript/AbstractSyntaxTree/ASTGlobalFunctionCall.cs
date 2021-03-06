﻿using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTGlobalFunctionCall : ASTFunctionCall
    {
        public ASTGlobalFunctionCall(string name, List<ASTExpressionBase> arguments) : base(name, arguments)
        {
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
