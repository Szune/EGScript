using System.Collections.Generic;
using EGScript.OperationCodes;

namespace EGScript.Scripter
{
    public class CodeBlock
    {
        private List<OperationCodeBase> _instructions { get;}

        public CodeBlock()
        {
            _instructions = new List<OperationCodeBase>();
        }

        public void Write(OperationCodeBase op)
        {
            _instructions.Add(op);
        }

        public OperationCodeBase this[int index] => _instructions[index];
        public int Count => _instructions.Count;
    }
}
