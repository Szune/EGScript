using System;

namespace EGScript.Helpers
{
    public class ConsolePrinter : IPrinter
    {
        public void Print(string toPrint)
        {
            Console.WriteLine(toPrint);
        }

        public void PrintException(string toPrint, Exception exception)
        {
            Console.WriteLine($"{toPrint}: {exception.Message}");
        }
    }
}
