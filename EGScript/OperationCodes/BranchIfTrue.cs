using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class BranchIfTrue : OperationCodeBase
    {
        public BranchIfTrue(uint argument)
        {
            Argument = argument;
        }

        public uint Argument { get; internal set; }

        public override void Execute(InterpreterState state)
        {
            if (state.Stack.Peek().Type == ObjectType.TRUE)
            {
                state.Frames.Peek().Address = (int)Argument;
            }
            state.Stack.Pop();
        }
    }
}
