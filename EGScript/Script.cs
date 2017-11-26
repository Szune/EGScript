﻿using EGScript.Helpers;
using EGScript.Objects;
using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript
{
    /// <summary>
    /// A script that can be run.
    /// </summary>
    public class Script
    {


        protected virtual ScriptObject Print(ScriptEnvironment env, List<ScriptObject> args)
        {
            _settings.Printer.Print(args[0].ToString());
            return ObjectFactory.Null;
        }

        protected virtual ScriptObject RandomNum(ScriptEnvironment env, List<ScriptObject> args)
        {
            switch(args.Count)
            {
                case 1:
                    {
                        if (!args[0].TryGetNumber(out Number n))
                            throw new ScriptException("random() only works with numbers.");
                        return new Number(_rand.Next((int)n.Value));
                    }
                case 2:
                    {
                        if (!args[0].TryGetNumber(out Number n1) || !args[0].TryGetNumber(out Number n2))
                            throw new ScriptException("random() only works with numbers.");
                        return new Number(_rand.Next((int)n1.Value, (int)n2.Value));
                    }
            }
            return ObjectFactory.Null;
        }

        protected virtual ScriptObject Error(ScriptEnvironment env, List<ScriptObject> args)
        {
            throw new ScriptException($"{args.First().ToString()}");
        }

        protected virtual void ExportGeneralFunctions()
        {
            _environment.ExportFunction(new ExportedFunction("print", Print, (1, 1)));
            _environment.ExportFunction(new ExportedFunction("random", RandomNum, (1, 2)));
            _environment.ExportFunction(new ExportedFunction("error", Error, (1, 1)));
        }

        private readonly Random _rand;
        private readonly ScriptEnvironment _environment; // environment contains the script (starting point = function main())
        private readonly Interpreter _interpreter; // interpreter runs the script
        private readonly Settings _settings;

        public Script(string code) : this(code, new FileHandler())
        {

        }

        public Script(string code, Settings settings) : this(code, settings, new FileHandler())
        {

        }

        public Script(string code, IFileHandler fileHandler)
        {
            // later: get lexer and parser from object pool
            // also get filehandler from object pool
            _rand = new Random();
            var lexer = new Lexer(code);
            var parser = new Parser(lexer, fileHandler);
            var ast = parser.ParseScript();
            _environment = new ScriptEnvironment();
            _settings = Settings.Default;
            // export functions here
            ExportGeneralFunctions();
            Compiler.Compile(_environment, ast); // from here on, you can reuse "_environment" (ScriptEnvironment),
                                                 // as an already compiled script (which will be important for performance later on,
                                                 // and there's really no point in compiling the same script over and over again)
            _interpreter = new Interpreter(_environment);
        }

        public Script(string code, Settings settings, IFileHandler fileHandler)
        {
            // later: get lexer and parser from object pool
            // also get filehandler from object pool
            _rand = new Random();
            var lexer = new Lexer(code);
            var parser = new Parser(lexer, fileHandler);
            var ast = parser.ParseScript();
            _environment = new ScriptEnvironment();
            _settings = settings;

            // export functions here
            ExportGeneralFunctions();
            foreach(var func in _settings.ExportedFunctions)
            {
                _environment.ExportFunction(func);
            }
            Compiler.Compile(_environment, ast); // from here on, you can reuse "_environment" (ScriptEnvironment),
                                                 // as an already compiled script (which will be important for performance later on,
                                                 // and there's really no point in compiling the same script over and over again)
            _interpreter = new Interpreter(_environment);
        }

        public ScriptObject Run(bool hideErrors = false)
        {
            if (!hideErrors)
                return _interpreter.Execute();
            else
                try
                {
                    return _interpreter.Execute();
                }
                catch
                {
                    return ObjectFactory.Null;
                }
        }
    }
}
