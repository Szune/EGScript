using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Objects
{
    public class ObjectFactory
    {
        public static readonly ScriptObject Null = new Null();

        public static readonly ScriptObject True = new True();

        public static readonly ScriptObject False = new False();

        public static StringObj String(string text)
        {
            return new StringObj(text);
        }

        public static Class Class(string name)
        {
            return new Class(name);
        }

        public static Class Class(string name, Class baseClass)
        {
            return new Class(name, baseClass);
        }

        public static Function Function(string name, List<string> arguments)
        {
            return new Function(name, arguments);
        }

        public static Number Number(double number)
        {
            return new Number(number);
        }

        public static Instance Instance(Class ofClass)
        {
            return new Instance(ofClass);
        }
    }
}
