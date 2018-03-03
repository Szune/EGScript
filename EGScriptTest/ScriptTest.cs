using System.Collections.Generic;
using System.Linq;
using EGScript;
using EGScript.Objects;
using EGScript.Scripter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EGScriptTest
{
    [TestClass]
    public class ScriptTest
    {
        private ScriptObject IncreaseBy3(ScriptEnvironment env, Stack<ScriptObject> args)
        {
            return new Number(args.Pop().As<Number>().Value + 3);
        }

        [TestMethod]
        public void ExportedFunction_Should_Be_Available_For_Use_In_Script()
        {
            var toIncrease = 6;
            var settings = new ScriptSettings(new List<ExportedFunction> { new ExportedFunction("increaseBy3", IncreaseBy3, (1, 1))});
            var script = new Script(@"function main()
{
    return increaseBy3(" + toIncrease + @");
}", settings);
            script.Run().As<Number>().Value.Should().Be(toIncrease + 3);
        }
    }
}
