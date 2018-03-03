using System.Collections.Generic;
using EGScript.Objects;

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
