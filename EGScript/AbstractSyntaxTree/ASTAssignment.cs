using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTAssignment : ASTExpressionBase
    {
        public enum AssignmentType
        {
            /// <summary>
            /// '='
            /// </summary>
            ASSIGNMENT,
            /// <summary>
            /// '+='
            /// </summary>
            ADDITION,
            /// <summary>
            /// '-='
            /// </summary>
            SUBTRACTION,
            /// <summary>
            /// '*='
            /// </summary>
            MULTIPLICATION,
            /// <summary>
            /// '/='
            /// </summary>
            DIVISION
        }

        public AssignmentType OperationType { get; }
        public override ExpressionType Type => ExpressionType.ASSIGNMENT;
        public string Variable { get; }
        public ASTExpressionBase Expression { get; }

        public ASTAssignment(AssignmentType opType, string variable, ASTExpressionBase expression)
        {
            OperationType = opType;
            Variable = variable;
            Expression = expression;
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }

        public ASTGlobalVariableAssignment ToGlobalVariable()
        {
            return new ASTGlobalVariableAssignment(Variable, Expression);
        }
    }
}
