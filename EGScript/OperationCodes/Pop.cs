using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class Pop : OperationCodeBase
    {
        public static readonly Pop Cached = new Pop();
        public override void Execute(InterpreterState state)
        {
            state.Stack.Pop();
        }
    }
}
