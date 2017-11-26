using EGScript.Helpers;
using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript
{
    public class Settings
    {

        public static Settings Default => new Settings();
        public IPrinter Printer;
        public IEnumerable<ExportedFunction> ExportedFunctions;
        
        public Settings()
        {
            Printer = new ConsolePrinter();
            ExportedFunctions = new List<ExportedFunction>();
        }
    }
}
