using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class Null : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            state.Stack.Push(ObjectFactory.Null);
        }
    }
}
