using System;
using System.Collections.Generic;
using System.Linq;
using EGScript.Helpers;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript
{
    /// <summary>
    /// A script that can be run.
    /// </summary>
    public class Script
    {
        private readonly string _code;
        private readonly IFileHandler _fileHandler;
        private readonly Random _rand;
        private readonly ScriptEnvironment _environment; // environment contains the script (starting point = function main())
        private Interpreter _interpreter; // interpreter runs the script
        private readonly ScriptSettings _settings;

        public Script(string code) : this(code, new FileHandler())
        {

        }

        public Script(string code, ScriptSettings settings) : this(code, settings, new FileHandler())
        {

        }

        public Script(string code, IFileHandler fileHandler)
        {
            _rand = new Random();
            _environment = new ScriptEnvironment();
            _code = code;
            _settings = ScriptSettings.Default;
            _fileHandler = fileHandler;

        }

        public Script(string code, ScriptSettings settings, IFileHandler fileHandler) : this(code, fileHandler)
        {
            _settings = settings;
        }

        public ScriptObject Run(bool throwExceptionOnError = true)
        {
            try
            {

                if (_interpreter != null) // if already compiled, just execute
                    return _interpreter.Execute();

                Compile();
                return _interpreter.Execute();
            }
            catch (Exception ex)
            {
                if (throwExceptionOnError)
                    throw;

                _settings?.Printer?.Print(ex.ToString());
                return ObjectFactory.Null;
            }
        }

        private void Compile()
        {
            // compile script
            var lexer = new Lexer(_code);
            var parser = new Parser(lexer, _fileHandler);
            var ast = parser.ParseScript();

            ExportGeneralFunctions();
            foreach (var func in _settings.Functions)
            {
                _environment.ExportFunction(func);
            }

            Compiler.Compile(_environment, ast);
            _interpreter = new Interpreter(_environment);
        }

        protected virtual ScriptObject Print(ScriptEnvironment env, Stack<ScriptObject> args)
        {
            _settings.Printer.Print(args.Pop().ToString());
            return null;
        }

        protected virtual ScriptObject RandomNum(ScriptEnvironment env, Stack<ScriptObject> args)
        {
            switch(args.Count)
            {
                case 1:
                {
                    if (!args.Pop().TryGetNumber(out Number n))
                        throw new ScriptException("random() only works with numbers.");
                    return new Number(_rand.Next((int)n.Value));
                }
                case 2:
                {
                    if (!args.Pop().TryGetNumber(out Number n1) || !args.Pop().TryGetNumber(out Number n2))
                        throw new ScriptException("random() only works with numbers.");
                    return new Number(_rand.Next((int)n1.Value, (int)n2.Value));
                }
                default:
                    return null;
            }
        }

        protected virtual ScriptObject Error(ScriptEnvironment env, Stack<ScriptObject> args)
        {
            throw new ScriptException(args.Pop().ToString());
        }

        private void ExportGeneralFunctions()
        {
            _environment.ExportFunction(new ExportedFunction("print", Print, (1, 1)));
            _environment.ExportFunction(new ExportedFunction("random", RandomNum, (1, 2)));
            _environment.ExportFunction(new ExportedFunction("error", Error, (1, 1)));
        }
    }
}
