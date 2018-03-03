using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class GreaterThan : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            var right = state.Stack.Peek();
            state.Stack.Pop();

            var left = state.Stack.Peek();
            state.Stack.Pop();

            if (left > right)
                state.Stack.Push(ObjectFactory.True);
            else
                state.Stack.Push(ObjectFactory.False);
        }
    }
}
