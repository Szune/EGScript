using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class Push : OperationCodeBase
    {
        public Push(ScriptObject toPush)
        {
            Object = toPush;
        }

        public ScriptObject Object { get; }

        public override void Execute(InterpreterState state)
        {
            state.Stack.Push(Object);
        }
    }
}
