using System;

namespace EGScript.Scripter
{
    public class ScopeException : Exception
    {
        public ScopeException(string message) : base($"Scope: {message}")
        {
        }
    }
}
