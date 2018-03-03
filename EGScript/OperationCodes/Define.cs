using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    /// <summary>
    /// Defines a variable.
    /// </summary>
    public class Define : OperationCodeBase
    {
        public Define(ScriptObject variableName)
        {
            VariableName = variableName;
        }

        public ScriptObject VariableName { get; }

        public override void Execute(InterpreterState state)
        {
            if (!VariableName.TryGetString(out StringObj s))
                throw new InterpreterException($"Instruction object was of type '{VariableName.TypeName}', expected 'string'.");

            state.Scopes.Peek().Define(s.Text, state.Stack.Peek());
        }
    }
}
