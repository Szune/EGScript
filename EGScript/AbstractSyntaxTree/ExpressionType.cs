using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.AbstractSyntaxTree
{
    public enum ExpressionType
    {
        NONE,
        ASSIGNMENT,
        COMPARE,
        IDENTIFIER,
        NUMBER,
        STRING,
        TABLE,
        NIL,
        TRUE,
        FALSE
    }
}
