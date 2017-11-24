using EGScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScriptTest
{
    public class TestPrinter : IPrinter
    {
        public List<string> PrintedMessages = new List<string>();
        public void Print(string toPrint)
        {
            PrintedMessages.Add(toPrint);
            Console.WriteLine(toPrint);
        }
    }
}
