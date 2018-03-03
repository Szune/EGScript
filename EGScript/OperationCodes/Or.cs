using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Or : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            var right = state.Stack.Peek();
            state.Stack.Pop();

            var left = state.Stack.Peek();
            state.Stack.Pop();

            switch (left.Type)
            {
                case ObjectType.TRUE:
                    {
                        switch (right.Type)
                        {
                            case ObjectType.TRUE:
                                {
                                    state.Stack.Push(ObjectFactory.True);
                                }
                                break;
                            case ObjectType.FALSE:
                                {
                                    state.Stack.Push(ObjectFactory.True);
                                }
                                break;
                            default:
                                throw new InterpreterException("Type mismatch on '||' operator.");
                        }
                    }
                    break;
                case ObjectType.FALSE:
                    {
                        switch (right.Type)
                        {
                            case ObjectType.TRUE:
                                {
                                    state.Stack.Push(ObjectFactory.True);
                                }
                                break;
                            case ObjectType.FALSE:
                                {
                                    state.Stack.Push(ObjectFactory.False);
                                }
                                break;
                            default:
                                throw new InterpreterException("Type mismatch on '||' operator.");
                        }
                    }
                    break;
                default:
                    throw new InterpreterException("Invalid arguments to '||' operator.");
            }
        }
    }
}
