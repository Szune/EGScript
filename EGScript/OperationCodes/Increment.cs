using EGScript.Objects;
using EGScript.Scripter;

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
