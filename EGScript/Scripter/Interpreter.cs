using EGScript.Objects;
using EGScript.OperationCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class Interpreter
    {
        private ScriptEnvironment _environment;
        private Stack<CallFrame> _frames;
        private Stack<ScriptObject> _stack;
        private Stack<Scope> _scopes;

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

            var state = new InterpreterState(_environment, _frames, _stack, _scopes);

            while (_frames.Count > 0)
            {
                var frame = _frames.Peek();
                var instruction = frame.Function.Code[frame.Address];

                frame.Address++;

                instruction.Execute(state);

                if (!(instruction is Return ret)) continue; // check if instruction was a return instruction
                if (ret.ScriptExecutionFinished) // check if it's returning from main()
                    return ret.ReturnObject; // return usable object
            }

            return ObjectFactory.Null;
        }
    }
}
