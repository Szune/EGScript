using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Pop : OperationCodeBase
    {
        public static readonly Pop Cached = new Pop();
        public override void Execute(InterpreterState state)
        {
            state.Stack.Pop();
        }
    }
}
