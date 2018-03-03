using System;

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
