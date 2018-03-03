using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    /// <summary>
    /// Negates a number.
    /// </summary>
    public class Negate : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            var value = state.Stack.Peek();
            state.Stack.Pop();

            switch (value.Type)
            {
                case ObjectType.NUMBER:
                    {
                        state.Stack.Push(new Number(-(((Number)value).Value)));
                    }
                    break;
                default:
                    {
                        throw new InterpreterException($"Invalid arguments to '-' operator.");
                    }
            }
        }
    }
}
