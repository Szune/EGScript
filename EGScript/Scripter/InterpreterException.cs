using System;
using System.Runtime.Serialization;

namespace EGScript.Scripter
{
    public class InterpreterException : Exception
    {

        public InterpreterException(string message) : base($"Interpreter: {message}")
        {
        }
    }
}