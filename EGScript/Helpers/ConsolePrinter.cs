using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Helpers
{
    public class ConsolePrinter : IPrinter
    {
        public void Print(string toPrint)
        {
            Console.WriteLine(toPrint);
        }
    }
}
