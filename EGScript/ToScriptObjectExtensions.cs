using System;
using System.Collections.Generic;
using System.Linq;
using EGScript.Objects;

namespace EGScript
{
    public static class ToScriptObjectExtensions
    {
        #region String
        public static ScriptObject ToScriptObject(this string text)
        {
            return new StringObj(text);
        }

        public static List<ScriptObject> ToScriptObjects(this IEnumerable<string> texts)
        {
            return texts.Select(t => new StringObj(t).As<ScriptObject>()).ToList();
        }

        public static List<ScriptObject> ToScriptObjects(this string[] texts)
        {
            return texts.Select(t => new StringObj(t).As<ScriptObject>()).ToList();
        }
        #endregion

        #region Double
        public static ScriptObject ToScriptObject(this double number)
        {
            return new Number(number);
        }

        public static List<ScriptObject> ToScriptObjects(this IEnumerable<double> numbers)
        {
            return numbers.Select(n => new Number(n).As<ScriptObject>()).ToList();
        }

        public static List<ScriptObject> ToScriptObjects(this double[] numbers)
        {
            return numbers.Select(n => new Number(n).As<ScriptObject>()).ToList();
        }
        #endregion

        #region Int
        public static ScriptObject ToScriptObject(this int number)
        {
            return new Number(number);
        }

        public static List<ScriptObject> ToScriptObjects(this IEnumerable<int> numbers)
        {
            return numbers.Select(n => new Number(n).As<ScriptObject>()).ToList();
        }

        public static List<ScriptObject> ToScriptObjects(this int[] numbers)
        {
            return numbers.Select(n => new Number(n).As<ScriptObject>()).ToList();
        }
        #endregion

        #region Object
        public static ScriptObject ToScriptObject(this object obj)
        {
            // Convert any variable into a scriptobject, to make it easy to push variables from C# to a script
            switch (obj)
            {
                case string str:
                    return ToScriptObject(str);
                case double doubleNum:
                    return ToScriptObject(doubleNum);
                case int intNum:
                    return ToScriptObject(intNum);
            }

            throw new NotImplementedException($"'{obj.GetType().Name}' is not supported yet.");
        }

        public static List<ScriptObject> ToScriptObjects(this IEnumerable<object> objects)
        {
            return objects.Select(ToScriptObject).ToList();
        }

        public static List<ScriptObject> ToScriptObjects(this object[] objects)
        {
            return objects.Select(ToScriptObject).ToList();
        }
        #endregion

        //#region Class
        //public static ScriptObject Object<TClass>(TClass obj) where TClass : class
        //{
        //    throw new NotImplementedException();
        //    // Convert any class into a script class, to make it easy to push variables from C# to a script
        //    // this may not be possible, but it's worth trying
        //    // structs should be possible though
        //}

        //public static List<ScriptObject> Objects<TClass>(IEnumerable<TClass> objects) where TClass : class
        //{
        //    return objects.Select(Object).ToList();
        //}

        //public static List<ScriptObject> Objects<TClass>(params TClass[] objects) where TClass : class
        //{
        //    return objects.Select(Object).ToList();
        //}
        //#endregion
    }
}
