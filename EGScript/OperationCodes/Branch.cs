using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class Branch : OperationCodeBase
    {
        public Branch(uint argument)
        {
            Argument = argument;
        }

        public uint Argument { get; internal set; }

        public override void Execute(InterpreterState state)
        {
            state.Frames.Peek().Address = (int)Argument;
        }
    }
}
