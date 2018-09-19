using System;

namespace EGScript.Helpers
{
    public interface IPrinter
    {
        void Print(string toPrint);
        void PrintException(string toPrint, Exception exception);
    }
}
