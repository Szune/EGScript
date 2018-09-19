using System;
using System.Collections.Generic;
using EGScript.Helpers;

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

        public void PrintException(string toPrint, Exception exception)
        {
            PrintedMessages.Add(toPrint);
            Console.WriteLine(toPrint + " -> exception: " + exception);
        }
    }
}
