using EGScript.Objects;
using EGScript.OperationCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
