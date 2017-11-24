using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class BranchIfFalse : OperationCodeBase
    {
        public BranchIfFalse(uint argument)
        {
            Argument = argument;
        }

        public uint Argument { get; internal set; }

        public override void Execute(InterpreterState state)
        {
            if (state.Stack.Peek().Type == ObjectType.FALSE)
            {
                state.Frames.Peek().Address = (int)Argument;
            }
            state.Stack.Pop();
        }
    }
}
