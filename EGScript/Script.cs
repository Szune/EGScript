using EGScript.Helpers;
using EGScript.Objects;
using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript
{
    public class Script
    {
        private ScriptObject Print(ScriptEnvironment env, List<ScriptObject> args)
        {
            Settings.Printer.Print(args[0].ToString());
            return null;
        }

        private ScriptObject RandomNum(ScriptEnvironment env, List<ScriptObject> args)
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
            return ScriptEnvironment.NullObject;
        }

        private void ExportGeneralFunctions()
        {
            _environment.ExportFunction(new ExportedFunction("print", Print, (1, 1)));
            _environment.ExportFunction(new ExportedFunction("random", RandomNum, (1, 2)));
        }

        private readonly Random _rand;
        private readonly ScriptEnvironment _environment; // environment contains the script (starting point = function main())
        private readonly Interpreter _interpreter; // interpreter runs the script

        public Script(string code) : this(code, new FileHandler())
        {

        }

        public Script(string code, List<ExportedFunction> exportedFunctions) : this(code, exportedFunctions, new FileHandler())
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
            // export functions here
            ExportGeneralFunctions();
            Compiler.Compile(_environment, ast); // from here on, you can reuse "_environment" (ScriptEnvironment),
                                                 // as an already compiled script (which will be important for performance later on,
                                                 // and there's really no point in compiling the same script over and over again)
            _interpreter = new Interpreter(_environment);
        }

        public Script(string code, List<ExportedFunction> exportedFunctions, IFileHandler fileHandler)
        {
            // later: get lexer and parser from object pool
            // also get filehandler from object pool
            _rand = new Random();
            var lexer = new Lexer(code);
            var parser = new Parser(lexer, fileHandler);
            var ast = parser.ParseScript();
            _environment = new ScriptEnvironment();

            // export functions here
            ExportGeneralFunctions();
            for(int i = 0; i < exportedFunctions.Count; i++)
            {
                _environment.ExportFunction(exportedFunctions[i]);
            }
            Compiler.Compile(_environment, ast); // from here on, you can reuse "_environment" (ScriptEnvironment),
                                                 // as an already compiled script (which will be important for performance later on,
                                                 // and there's really no point in compiling the same script over and over again)
            _interpreter = new Interpreter(_environment);
        }

        public ScriptObject Run()
        {
            return _interpreter.Execute();
        }
    }
}
