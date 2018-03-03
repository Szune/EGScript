using System;
using System.Collections.Generic;
using EGScript.Objects;

namespace EGScript.Scripter
{
    /// <summary>
    /// A container for the delegate to call and metadata to improve debugging.
    /// </summary>
    public class ExportedFunction
    {
        /// <summary>
        /// Gets the delegate to call the exported function with.
        /// </summary>
        public Func<ScriptEnvironment, Stack<ScriptObject>, ScriptObject> Call { get; }
        /// <summary>
        /// Gets the required amount of parameters to call the exported function.
        /// </summary>
        public (int Min, int Max) ArgumentCount { get; }

        /// <summary>
        /// Gets the name used when calling the function.
        /// </summary>
        public string FunctionName { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="functionName">The name used when calling the function.</param>
        /// <param name="functionToExport">The function to export.</param>
        /// <param name="numberOfArguments">The required amount of parameters to call the exported function.</param>
        public ExportedFunction(string functionName, Func<ScriptEnvironment, Stack<ScriptObject>, ScriptObject> functionToExport, (int min, int max) numberOfArguments)
        {
            FunctionName = functionName;
            Call = functionToExport;
            ArgumentCount = numberOfArguments;
        }
    }
}
