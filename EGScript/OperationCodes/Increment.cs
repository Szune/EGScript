using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class Increment : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            var identifier = state.Stack.Peek();
            switch (identifier.Type)
            {
                case ObjectType.NUMBER:
                    {
                        var num = (Number)identifier;
                        num.Set(num.Value + 1);
                    }
                    break;
                case ObjectType.NULL:
                    throw new InterpreterException("Attempt to increment an uninitialized variable.");
                default:
                    throw new InterpreterException("Attempt to increment invalid value.");
            }
        }
    }
}
