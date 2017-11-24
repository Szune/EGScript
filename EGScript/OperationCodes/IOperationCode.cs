using EGScript.Objects;
using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.OperationCodes
{
    public interface IOperationCode
    {
        void Execute(InterpreterState state);
    }
}
