using EGScript.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public struct InterpreterState
    {

        public ScriptEnvironment Environment { get; }
        public Stack<CallFrame> Frames { get; }
        public Stack<ScriptObject> Stack { get; }
        public Stack<Scope> Scopes { get; }

        public InterpreterState(ScriptEnvironment environment, Stack<CallFrame> frames, Stack<ScriptObject> stack, Stack<Scope> scopes)
        {
            Environment = environment;
            Frames = frames;
            Stack = stack;
            Scopes = scopes;
        }
    }
}
