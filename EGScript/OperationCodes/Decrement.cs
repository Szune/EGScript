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
    /// Decrements a variable by 1.
    /// </summary>
    public class Decrement : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            var identifier = state.Stack.Peek();
            switch (identifier.Type)
            {
                case ObjectType.NUMBER:
                    {
                        var num = (Number)identifier;
                        num.Set(num.Value - 1);
                    }
                    break;
                case ObjectType.NULL:
                    throw new InterpreterException("Attempt to decrement an uninitialized variable.");
                default:
                    throw new InterpreterException("Attempt to decrement invalid value.");
            }
        }
    }
}
