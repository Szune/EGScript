using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Set : OperationCodeBase
    {
        public Set(ScriptObject variableName)
        {
            _variableName = variableName;
        }

        private readonly ScriptObject _variableName;
        public override void Execute(InterpreterState state)
        {
            if (!_variableName.TryGetString(out StringObj s))
                throw new InterpreterException($"Instruction object was of type '{_variableName.TypeName}', expected 'string'.");

            var scope = state.Scopes.Peek();
            scope.Set(s.Text, state.Stack.Peek());
        }
    }
}
