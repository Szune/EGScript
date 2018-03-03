using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Multiply : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            var right = state.Stack.Peek();
            state.Stack.Pop();

            var left = state.Stack.Peek();
            state.Stack.Pop();

            switch (left.Type)
            {
                case ObjectType.NUMBER:
                    {
                        switch (right.Type)
                        {
                            case ObjectType.NUMBER:
                                {
                                    state.Stack.Push(new Number(((Number)left).Value * ((Number)right).Value));
                                }
                                break;
                            default:
                                {
                                    throw new InterpreterException($"Type mismatch on '*' operator.");
                                }
                        }
                    }
                    break;
                default:
                    {
                        throw new InterpreterException($"Invalid arguments to '*' operator.");
                    }
            }
        }
    }
}
