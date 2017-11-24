using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.AbstractSyntaxTree
{
    /// <summary>
    /// Abstract Syntax Tree representation.
    /// </summary>
    public class AST : IVisitable
    {
        public List<ASTClassDefinition> Classes { get; }
        public List<ASTFunctionBase> Functions { get; }
        public List<ASTGlobalVariableAssignment> GlobalVars { get; }

        public AST(List<ASTClassDefinition> classes, List<ASTFunctionBase> functions, List<ASTGlobalVariableAssignment> globalVars)
        {
            Classes = classes;
            Functions = functions;
            GlobalVars = globalVars;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
