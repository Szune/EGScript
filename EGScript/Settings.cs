using EGScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript
{
    public sealed class Settings
    {

        public static Settings Current { get; private set; }
        private IPrinter printer; // will be reset if settings are reset

        static Settings()
        {
            Current = new Settings();
        }

        private Settings()
        {
            printer = new ConsolePrinter();
        }

        public static void Reset()
        {
            Current = new Settings();
        }

        public static IPrinter Printer {
            get
            {
                return Current.printer;
            }
            set
            {
                if (value != null)
                    Current.printer = value;
            }
        }

    }
}
