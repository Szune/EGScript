using System;

namespace EGScript.Scripter
{
    public class ScriptEnvironmentException : Exception
    {
        public ScriptEnvironmentException(string message) : base($"Environment: {message}")
        {
        }
    }
}
