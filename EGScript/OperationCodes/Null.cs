using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Null : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            state.Stack.Push(ObjectFactory.Null);
        }
    }
}
