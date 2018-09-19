using EGScript.Objects;
using EGScript.Scripter;
using System;

namespace EGScript.OperationCodes
{
    public class SetMember : OperationCodeBase
    {
        private readonly StringObj _memberName;
        private readonly StringObj _instanceName;

        public SetMember(StringObj memberName, StringObj instanceName)
        {
            _memberName = memberName;
            _instanceName = instanceName;
        }

        public SetMember(StringObj memberName) : this(memberName, null)
        {
        }

        public override void Execute(InterpreterState state)
        {
            if (_instanceName != null) // find instance using variable name
                state.Scopes.Peek().Find(_instanceName.Text).As<Instance>().Scope.Set(_memberName.Text, state.Stack.Peek());
            else // current scope is function scope, parent should be class scope
                state.Scopes.Peek().Parent.Set(_memberName.Text, state.Stack.Peek());
        }
    }
}
