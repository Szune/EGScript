using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class Divide : OperationCodeBase
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
                                    if (((Number)right).Value == 0)
                                        state.Stack.Push(ObjectFactory.Null);
                                    else
                                        state.Stack.Push(new Number(((Number)left).Value / ((Number)right).Value));
                                }
                                break;
                            default:
                                {
                                    throw new InterpreterException($"Type mismatch on '/' operator.");
                                }
                        }
                    }
                    break;
                default:
                    {
                        throw new InterpreterException($"Invalid arguments to '/' operator.");
                    }
            }
        }
    }
}
