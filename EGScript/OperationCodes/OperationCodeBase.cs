using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    public abstract class OperationCodeBase : IOperationCode
    {
        public abstract void Execute(InterpreterState state);
        public TOpCode As<TOpCode>() where TOpCode : OperationCodeBase
        {
            return this as TOpCode;
        }
    }
}
