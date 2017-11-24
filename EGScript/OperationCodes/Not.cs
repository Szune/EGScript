using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

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
