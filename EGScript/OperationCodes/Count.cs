using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    /// <summary>
    /// Pushes the amount elements in a table.
    /// </summary>
    public class Count : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            if (!state.Stack.Peek().TryGetTable(out Table t))
                throw new InterpreterException($"expected 'table' object on top of stack.");
            state.Stack.Pop();
            state.Stack.Push(new Number(t.Count)); // push table element count
        }
    }
}
