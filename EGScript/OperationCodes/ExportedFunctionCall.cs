using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class ExportedFunctionCall : OperationCodeBase
    {
        public ExportedFunctionCall(ScriptObject functionName)
        {
            FunctionName = functionName;
        }

        public ScriptObject FunctionName { get; }

        public override void Execute(InterpreterState state)
        {
            if (!state.Stack.Peek().TryGetNumber(out Number n))
                throw new InterpreterException($"Object on top of stack was of type '{state.Stack.Peek().TypeName}', expected 'number'.");

            double numArgs = n.Value;
            state.Stack.Pop();

            var args = new List<ScriptObject>();
            for (int i = 0; i < numArgs; i++)
            {
                args.Add(state.Stack.Peek()); // What's the reason for not just using _stack.Pop() in the call to args.Add()? Am I too tired to reason well or is this unnecessary?
                state.Stack.Pop();
            }

            if (!FunctionName.TryGetString(out StringObj s))
                throw new InterpreterException($"Instruction object was of type '{FunctionName.TypeName}', expected 'string'.");

            var exportedFunction = state.Environment.FindExportedFunction(s.Text);

            if (exportedFunction == null)
                throw new InterpreterException($"exported function '{s.Text}' does not exist.");

            if (args.Count < exportedFunction.ArgumentCount.Min || args.Count > exportedFunction.ArgumentCount.Max)
                throw new InterpreterException($"exported function '{s.Text}' requires {exportedFunction.ArgumentCount} argument(s).");

            var returnValue = exportedFunction.Call(state.Environment, args);
            if (returnValue != null)
                state.Stack.Push(returnValue);
            else
                state.Stack.Push(ObjectFactory.Null);
        }
    }
}
