using EGScript.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public enum OperationCode
    {
        /// <summary>
        /// Push object onto top of stack.
        /// </summary>
        PUSH,
        /// <summary>
        /// Pop object from top of stack.
        /// </summary>
        POP,
        /// <summary>
        /// Push null onto stack.
        /// </summary>
        NULL,

        /// <summary>
        /// Call a function.
        /// </summary>
        CALL,
        /// <summary>
        /// Call a member function.
        /// </summary>
        MCALL,
        /// <summary>
        /// Return from a function call, a return value is on top of the stack.
        /// </summary>
        RETURN,
        /// <summary>
        /// Call exported function.
        /// </summary>
        ECALL,

        /// <summary>
        /// Sets variable to object on top of the stack.
        /// </summary>
        SET,
        /// <summary>
        /// Defines a variable and sets it to object on top of the stack.
        /// </summary>
        DEF,
        /// <summary>
        /// References a variable to the stack.
        /// </summary>
        REF,
        /// <summary>
        /// Creates a table of the objects on top of the stack.
        /// </summary>
        MAKE_TABLE,
        /// <summary>
        /// Push element count of object to top of stack.
        /// </summary>
        COUNT,

        /// <summary>
        /// Branch unconditionally.
        /// </summary>
        BRANCH,
        /// <summary>
        /// If top of the stack has True then branch.
        /// </summary>
        BRANCH_IF_TRUE,
        /// <summary>
        /// If top of the stack has False then branch.
        /// </summary>
        BRANCH_IF_FALSE,

        /// <summary>
        /// Add two objects from top of stack, and push the answer to the stack.
        /// </summary>
        ADD,
        /// <summary>
        /// Subtract two objects from top of stack, and push the answer to the stack.
        /// </summary>
        SUB,
        /// <summary>
        /// Multiply two objects from top of stack, and push the answer to the stack.
        /// </summary>
        MUL,
        /// <summary>
        /// Divide two objects from top of stack, and push the answer to the stack.
        /// </summary>
        DIV,
        /// <summary>
        /// Negate the top of the stack.
        /// </summary>
        NEG,

        /// <summary>
        /// Increment the object on top of the stack.
        /// </summary>
        INC,
        /// <summary>
        /// Decrement the object on top of the stack.
        /// </summary>
        DEC,

        /// <summary>
        /// Logical not the top of the stack.
        /// </summary>
        NOT,
        /// <summary>
        /// Or two objects from top of stack, and push the answer to the stack.
        /// </summary>
        OR,
        /// <summary>
        /// And two objects from top of stack, and push the answer to the stack.
        /// </summary>
        AND,
        /// <summary>
        /// Compare two objects (==) from top of stack, and push the boolean answer to the stack.
        /// </summary>
        EQEQ,
        /// <summary>
        /// Compare two objects (!=) from top of stack, and push the boolean answer to the stack.
        /// </summary>
        NEQ,
        /// <summary>
        /// Compare two objects (&lt;) from top of stack, and push the boolean answer to the stack.
        /// </summary>
        LT,
        /// <summary>
        /// Compare two objects (&gt;) from top of stack, and push the boolean answer to the stack.
        /// </summary>
        GT,
        /// <summary>
        /// Compare two objects (&lt;=) from top of stack, and push the boolean answer to the stack.
        /// </summary>
        LTE,
        /// <summary>
        /// Compare two objects (&gt;=) from top of stack, and push the boolean answer to the stack.
        /// </summary>
        GTE
    }

    public class Instruction
    {
        public OperationCode OpCode;
        public uint Argument;
        public Scope Scope;
        public ScriptObject Object;

        public Instruction(OperationCode op)
        {
            OpCode = op;
            Argument = default(uint);
            Scope = default(Scope);
            Object = default(ScriptObject);
        }

        public Instruction(OperationCode op, uint argument) : this(op)
        {
            Argument = argument;
        }

        public Instruction(OperationCode op, ScriptObject obj) : this(op)
        {
            Object = obj;
        }

        public Instruction(OperationCode op, Scope scope) : this(op)
        {
            Scope = scope;
        }

        public Instruction(OperationCode op, Scope scope, ScriptObject obj) : this(op, scope)
        {
            Object = obj;
        }

        public Instruction(OperationCode op, uint argument, ScriptObject obj) : this(op, argument)
        {
            Object = obj;
        }

        public override string ToString()
        {
            return $"OpCode: {OpCode.ToString()}";
        }
    }
}
