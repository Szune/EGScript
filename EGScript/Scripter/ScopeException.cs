using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class ScopeException : Exception
    {
        public ScopeException(string message) : base($"Scope: {message}")
        {
        }
    }
}
