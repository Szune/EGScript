using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    /// <summary>
    /// Adds two strings/numbers.
    /// </summary>
    public class Add : OperationCodeBase
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
                                    //if (!left.TryGetNumber(out Number lNum) || !right.TryGetNumber(out Number rNum))
                                    //    throw new InterpreterException($"What just happened?");
                                    state.Stack.Push(new Number(((Number)left).Value + ((Number)right).Value));
                                }
                                break;
                            case ObjectType.STRING:
                                {
                                    state.Stack.Push(new StringObj(((Number)left).Value + ((StringObj)right).Text));
                                }
                                break;
                            default:
                                {
                                    throw new InterpreterException($"Type mismatch on '+' operator.");
                                }
                        }
                    }
                    break;
                case ObjectType.STRING:
                    {
                        switch (right.Type)
                        {
                            case ObjectType.STRING:
                                {
                                    //if (!left.TryGetString(out StringObj lStr) || !right.TryGetString(out StringObj rStr))
                                    //    throw new InterpreterException($"What just happened?");
                                    state.Stack.Push(new StringObj(((StringObj)left).Text + ((StringObj)right).Text));
                                }
                                break;
                            case ObjectType.NUMBER:
                                {
                                    state.Stack.Push(new StringObj(((StringObj)left).Text + ((Number)right).Value));
                                }
                                break;
                            default:
                                {
                                    throw new InterpreterException($"Type mismatch on '+' operator.");
                                }
                        }
                    }
                    break;
                default:
                    {
                        throw new InterpreterException($"Invalid arguments to '+' operator.");
                    }
            }
        }
    }
}
