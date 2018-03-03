using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Push : OperationCodeBase
    {
        public Push(ScriptObject toPush)
        {
            Object = toPush;
        }

        public ScriptObject Object { get; }

        public override void Execute(InterpreterState state)
        {
            state.Stack.Push(Object);
        }
    }
}
