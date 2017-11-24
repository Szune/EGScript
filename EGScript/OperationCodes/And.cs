using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class And : OperationCodeBase
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
                                    state.Stack.Push(ObjectFactory.False);
                                }
                                break;
                            default:
                                throw new InterpreterException("Type mismatch on '&&' operator.");
                        }
                    }
                    break;
                case ObjectType.FALSE:
                    {
                        switch (right.Type)
                        {
                            case ObjectType.TRUE:
                                {
                                    state.Stack.Push(ObjectFactory.False);
                                }
                                break;
                            case ObjectType.FALSE:
                                {
                                    state.Stack.Push(ObjectFactory.False);
                                }
                                break;
                            default:
                                throw new InterpreterException("Type mismatch on '&&' operator.");
                        }
                    }
                    break;
                default:
                    throw new InterpreterException("Invalid arguments to '&&' operator.");
            }
        }
    }
}
