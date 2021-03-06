﻿using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTMemberFunctionCall : ASTFunctionCall
    {
        public string InstanceName { get; }

        public ASTMemberFunctionCall(string name, string instanceName, List<ASTExpressionBase> arguments) : base(name, arguments)
        {
            InstanceName = instanceName;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
