using System.Collections.Generic;
using EGScript.Scripter;

namespace EGScript.Objects
{
    public class Function
    {
        public string Name { get; }
        public List<string> Arguments { get; }
        public CodeBlock Code { get; }
        public Scope Scope { get; private set; }

        public Function(string name)
        {
            Name = name;
            Scope = new Scope();
            Code = new CodeBlock();
        }
        public Function(string name, List<string> arguments)
        {
            Name = name;
            Arguments = arguments;
            Scope = new Scope();
            Code = new CodeBlock();
            for(int i = 0; i < arguments.Count; i++)
            {
                Scope.Define(arguments[i]);
            }
        }

        public void SetScope(Scope scope)
        {
            Scope = scope;
        }

    }
}
