using System.Collections.Generic;
using EGScript.Objects;
using EGScript.OperationCodes;

namespace EGScript.Scripter
{
    public class Interpreter
    {
        private readonly ScriptEnvironment _environment;
        private readonly Stack<CallFrame> _frames;
        private readonly Stack<ScriptObject> _stack;
        private readonly Stack<Scope> _scopes;

        public Interpreter(ScriptEnvironment environment)
        {
            _frames = new Stack<CallFrame>();
            _stack = new Stack<ScriptObject>();
            _scopes = new Stack<Scope>();
            _environment = environment;
        }

        public ScriptObject Execute()
        {
            var func = _environment.FindFunction("main");

            if (func == null)
                throw new InterpreterException("function main() does not exist.");

            _frames.Push(new CallFrame(func));
            _scopes.Push(func.Scope);

            // set up globals
            if (_environment.Globals.Code.Count > 0)
            {
                _environment.Globals.Code.Write(OpCodeFactory.Return); // return to main function afterwards
                _frames.Push(new CallFrame(_environment.Globals));
                _scopes.Push(_environment.Globals.Scope);
            }

            var state = new InterpreterState(_environment, _frames, _stack, _scopes);


            while (_frames.Count > 0)
            {
                var frame = _frames.Peek();
                var instruction = frame.Function.Code[frame.Address];

                frame.Address++;

                instruction.Execute(state);

                if (instruction is Return ret && ret.ScriptExecutionFinished) // check if instruction was a return instruction
                    return ret.ReturnObject; // return usable object
            }

            return ObjectFactory.Null;
        }
    }
}
