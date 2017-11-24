using EGScript.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class CodeBlock
    {
        private List<Instruction> _instructions { get; }
        public CodeBlock()
        {
            _instructions = new List<Instruction>();
        }

        public void Write(OperationCode op)
        {
            _instructions.Add(new Instruction(op));
        }

        public void Write(OperationCode op, uint argument)
        {
            _instructions.Add(new Instruction(op, argument));
        }

        public void Write(OperationCode op, ScriptObject obj)
        {
            _instructions.Add(new Instruction(op, obj));
        }

        public void Write(OperationCode op, Scope scope)
        {
            _instructions.Add(new Instruction(op, scope));
        }

        public void Write(OperationCode op, Scope scope, ScriptObject obj)
        {
            _instructions.Add(new Instruction(op, scope, obj));
        }

        public void Write(OperationCode op, uint argument, ScriptObject obj)
        {
            _instructions.Add(new Instruction(op, argument, obj));
        }

        public Instruction this[int index] => _instructions[index];
        public int Count => _instructions.Count;
    }
}
