using EGScript.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    /// <summary>
    /// A container for the delegate to call and metadata to improve debugging.
    /// </summary>
    public class ExportedFunction
    {
        public delegate ScriptObject EFunc(ScriptEnvironment environment, List<ScriptObject> arguments);

        /// <summary>
        /// Gets the delegate to call the exported function with.
        /// </summary>
        public EFunc Call { get; }
        /// <summary>
        /// Gets the required amount of parameters to call the exported function.
        /// </summary>
        public (int Min, int Max) ArgumentCount { get; }

        /// <summary>
        /// Gets the name used when calling the function.
        /// </summary>
        public string CallingName { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="callingName">The name used when calling the function.</param>
        /// <param name="functionToExport">The function to export.</param>
        /// <param name="numberOfArguments">The required amount of parameters to call the exported function.</param>
        public ExportedFunction(string callingName, EFunc functionToExport, (int min, int max) numberOfArguments)
        {
            CallingName = callingName;
            Call = functionToExport;
            ArgumentCount = numberOfArguments;
        }
    }
}
