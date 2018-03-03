using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class FunctionCall : OperationCodeBase
    {
        public FunctionCall(ScriptObject functionName)
        {
            FunctionName = functionName;
        }

        public ScriptObject FunctionName { get; }

        public override void Execute(InterpreterState state)
        {
            if (!FunctionName.TryGetString(out StringObj s))
                throw new InterpreterException($"Instruction object was of type '{FunctionName.TypeName}', expected 'string'.");

            var callFunction = state.Environment.FindFunction(s.Text);

            if (callFunction == null)
                throw new InterpreterException($"function '{s.Text}' does not exist.");

            state.Frames.Push(new CallFrame(callFunction));
            callFunction.Scope.Reset();
            state.Scopes.Push(new Scope(callFunction.Scope)); // TODO: Figure out if this is supposed to be using Scope.Copy() instead
        }
    }
}
