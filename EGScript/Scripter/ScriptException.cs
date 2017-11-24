using System;
using System.Runtime.Serialization;

namespace EGScript.Scripter
{
    public class ScriptException : Exception
    {
        public ScriptException(string message) : base($"Script: {message}")
        {
        }
    }
}