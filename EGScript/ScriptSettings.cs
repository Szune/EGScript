using System.Collections.Generic;
using EGScript.Helpers;
using EGScript.Scripter;

namespace EGScript
{
    public class ScriptSettings
    {

        public static ScriptSettings Default => new ScriptSettings();
        public IPrinter Printer { get; }
        private readonly List<ExportedFunction> _functions;
        public IEnumerable<ExportedFunction> Functions => _functions;
        
        public ScriptSettings() : this(new ConsolePrinter())
        {
        }

        public ScriptSettings(IPrinter printer)
        {
            Printer = printer;
            _functions = new List<ExportedFunction>();
        }

        public ScriptSettings(IEnumerable<ExportedFunction> functions)
        {
            _functions = new List<ExportedFunction>();
            _functions.AddRange(functions);
        }

        public ScriptSettings(IEnumerable<ExportedFunction> functions, IPrinter printer) : this(functions)
        {
            Printer = printer;
        }

        public void Add(ExportedFunction function)
        {
            _functions.Add(function);
        }
    }
}
