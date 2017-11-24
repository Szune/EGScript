using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTTableGet : ASTExpressionBase
    {
        public ASTExpressionBase Variable { get; }
        public List<ASTExpressionBase> TableIndexes { get; }

        public ASTTableGet(ASTExpressionBase variable, ASTExpressionBase tableIndex)
        {
            Variable = variable;
            TableIndexes = new List<ASTExpressionBase> { tableIndex };
        }

        public ASTTableGet(ASTExpressionBase variable, List<ASTExpressionBase> tableIndexes)
        {
            Variable = variable;
            TableIndexes = tableIndexes;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }
    }
}
