using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class Return : OperationCodeBase
    {
        public bool ScriptExecutionFinished;
        public ScriptObject ReturnObject;

        public static readonly Return Cached = new Return();

        public override void Execute(InterpreterState state)
        {
            if (state.Frames.Peek().Function?.Name == "main") // if function main() returns an object, return the value to the C# object calling the script
            {
                ScriptExecutionFinished = true;
                ReturnObject = state.Stack.Pop();
            }
            state.Frames.Pop();
            state.Scopes.Pop();
        }
    }
}
