using EGScript.Objects;
using EGScript.Scripter;
using System;

namespace EGScript.OperationCodes
{
    public class MemberAccess : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            // TODO add helpful exceptions if instance or memberName are of the wrong type
            var instance = state.Stack.Peek();
            state.Stack.Pop();

            var memberName = state.Stack.Peek();
            state.Stack.Pop();

            var value = instance.As<Instance>().Scope.Find(memberName.As<StringObj>().Text);

            state.Stack.Push(value);
        }
    }
}
