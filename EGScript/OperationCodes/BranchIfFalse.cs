using EGScript.Objects;
using EGScript.Scripter;

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
