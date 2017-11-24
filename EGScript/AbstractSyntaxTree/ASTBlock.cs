﻿using EGScript.Objects;
using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTBlock : ASTStatementBase
    {
        public List<ASTStatementBase> Statements { get; }
        public ASTBlock(List<ASTStatementBase> block)
        {
            Statements = block;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}