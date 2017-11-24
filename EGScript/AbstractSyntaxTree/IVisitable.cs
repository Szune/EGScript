using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.AbstractSyntaxTree
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
