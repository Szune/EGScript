using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public interface IOperationCode
    {
        void Execute(InterpreterState state);
    }
}
