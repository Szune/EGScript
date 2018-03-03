using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Global : OperationCodeBase
    {
        public Global(ScriptObject variableName)
        {
            VariableName = variableName;
        }

        public ScriptObject VariableName { get; }

        public override void Execute(InterpreterState state)
        {
            var value = state.Stack.Pop();
            var defined = state.Environment.Globals.Scope.Find(VariableName.ToString());
            if (defined != null)
            {
                if (defined != value)
                {
                    throw new InterpreterException($"Initializing a global variable ('{VariableName}') with two different values ('{defined}' and '{value}') is not possible.");
                }
                return;
            }
            state.Environment.Globals.Scope.Define(VariableName.ToString(), value);
        }
    }
}
