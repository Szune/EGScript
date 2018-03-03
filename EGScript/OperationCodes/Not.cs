using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Not : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            var value = state.Stack.Peek();
            state.Stack.Pop();

            switch (value.Type)
            {
                case ObjectType.TRUE:
                    {
                        state.Stack.Push(ObjectFactory.False);
                    }
                    break;
                case ObjectType.FALSE:
                    {
                        state.Stack.Push(ObjectFactory.True);
                    }
                    break;
                default:
                    throw new InterpreterException("Invalid arguments to '!' operator.");
            }
        }
    }
}
