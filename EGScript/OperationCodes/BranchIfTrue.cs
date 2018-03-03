using EGScript.Objects;
using EGScript.Scripter;

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
