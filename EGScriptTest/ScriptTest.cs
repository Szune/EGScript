using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EGScript;
using EGScript.Objects;
using EGScript.Scripter;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace EGScriptTest
{
    [TestClass]
    public class ScriptTest
    {
        private ScriptObject IncreaseBy3(ScriptEnvironment env, List<ScriptObject> args)
        {
            return new Number(args.First().As<Number>().Value + 3);
        }

        [TestMethod]
        public void ExportedFunction_Should_Be_Available_For_Use_In_Script()
        {
            var toIncrease = 6;
            var settings = new Settings { ExportedFunctions = new List<ExportedFunction> { new ExportedFunction("increaseBy3", IncreaseBy3, (1, 1)) } };
            var script = new Script(@"function main()
{
    return increaseBy3(" + toIncrease + @");
}", settings);
            script.Run().As<Number>().Value.Should().Be(toIncrease + 3);
        }
    }
}
