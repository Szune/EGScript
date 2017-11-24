using EGScript.Objects;
using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.AbstractSyntaxTree
{
    public abstract class ASTStatementBase
    {
        public abstract void Accept(IVisitor visitor, Function function);
    }
}
