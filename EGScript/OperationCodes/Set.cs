using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Set : OperationCodeBase
    {
        public Set(ScriptObject variableName)
        {
            VariableName = variableName;
        }

        public ScriptObject VariableName { get; }

        public override void Execute(InterpreterState state)
        {
            if (!VariableName.TryGetString(out StringObj s))
                throw new InterpreterException($"Instruction object was of type '{VariableName.TypeName}', expected 'string'.");

            state.Scopes.Peek().Set(s.Text, state.Stack.Peek());
        }
    }
}
