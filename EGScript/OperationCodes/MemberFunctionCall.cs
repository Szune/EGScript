﻿using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public class MemberFunctionCall : OperationCodeBase
    {
        public MemberFunctionCall(ScriptObject functionName)
        {
            _functionName = functionName;
        }

        private readonly ScriptObject _functionName;

        public override void Execute(InterpreterState state)
        {
            if (!state.Stack.Peek().TryGetNumber(out Number n))
                throw new InterpreterException($"Object on top of stack was of type '{state.Stack.Peek().TypeName}', expected 'number'.");

            double numArgs = n.Value;
            state.Stack.Pop();

            var instance = state.Stack.Peek();
            //_stack.Pop(); <- why is this part commented out in the original C++ code?

            if (instance.Type != ObjectType.INSTANCE)
                throw new InterpreterException($"Member call expected class instance.");

            var _class = (instance as Instance).Class;
            if (!_functionName.TryGetString(out StringObj s))
                throw new InterpreterException($"Instruction object was of type '{_functionName.TypeName}', expected 'string'.");

            var callFunction = _class.FindFunction(s.Text);
            if (callFunction == null)
                throw new InterpreterException($"Class '{_class.Name}' does not define function '{s.Text}'.");

            state.Frames.Push(new CallFrame(callFunction, instance.As<Instance>()));
            callFunction.Scope.Reset();
            callFunction.Scope.SetParent(_class.Scope);
            state.Scopes.Push(callFunction.Scope);
        }
    }
}
