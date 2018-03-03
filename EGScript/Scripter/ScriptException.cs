using System;

namespace EGScript.Scripter
{
    public class ScriptException : Exception
    {
        public ScriptException(string message) : base($"Script: {message}")
        {
        }
    }
}