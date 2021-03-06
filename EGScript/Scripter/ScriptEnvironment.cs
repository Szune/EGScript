﻿using System.Collections.Generic;
using System.Linq;
using EGScript.Objects;
using EGScript.OperationCodes;

namespace EGScript.Scripter
{
    public class ScriptEnvironment
    {
        private List<Class> _classes { get; }
        private List<Function> _functions { get; }
        public Dictionary<string, ExportedFunction> exportedFunctions;
        public Function Globals = new Function("");


        public ScriptEnvironment()
        {
            _classes = new List<Class>();
            _functions = new List<Function>();
            exportedFunctions = new Dictionary<string, ExportedFunction>();

            CreateGeneralClasses();
        }

        private void CreateGeneralClasses()
        {
            // TODO: work on class implementation
            var table = new Class("table");
            var func = new Function("count", new List<string> { "tname" });
            func.Code.Write(OpCodeFactory.Set(ObjectFactory.String("tname")));
            func.Code.Write(OpCodeFactory.Pop);
            func.Code.Write(OpCodeFactory.Reference(ObjectFactory.String("tname")));
            func.Code.Write(OpCodeFactory.Return);
            table.AddFunction(func);
            var constructor = new Function("table", new List<string>());
            constructor.Code.Write(OpCodeFactory.Push(ObjectFactory.Null));
            constructor.Code.Write(OpCodeFactory.Return);
            table.AddFunction(constructor);
            _classes.Add(table);
        }

        public void AddClass(Class classToAdd)
        {
            _classes.Add(classToAdd);
        }

        public void AddFunction(Function functionToAdd)
        {
            _functions.Add(functionToAdd);
        }

        /// <summary>
        /// Go to the definition of this method to find out how exporting a C# method to the script language works.
        /// </summary>
        /// <param name="exportedFunction"></param>
        public void ExportFunction(ExportedFunction exportedFunction)
        {

            exportedFunctions.Add(exportedFunction.FunctionName, exportedFunction);
        }

        public Class FindClass(string name)
        {
            return _classes.FirstOrDefault(_ => _.Name == name);
        }

        public Function FindFunction(string name)
        {
            return _functions.FirstOrDefault(_ => _.Name == name);
        }

        public ExportedFunction FindExportedFunction(string name)
        {
            if (exportedFunctions.TryGetValue(name, out ExportedFunction func))
                return func;
            return null;
        }
    }
}
