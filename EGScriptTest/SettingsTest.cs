using System.Collections.Generic;
using EGScript;
using EGScript.Scripter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EGScriptTest
{
    [TestClass]
    public class SettingsTest
    {
        ScriptSettings settings;
        TestPrinter printer => settings.Printer as TestPrinter;

        [TestInitialize]
        public void InitializeTest()
        {
            settings = new ScriptSettings(new TestPrinter());
        }

        [TestMethod]
        public void ExportedFunctions_Should_Have_Count_Of_Zero_By_Default()
        {
            settings.Functions.Should().HaveCount(0);
        }

        [TestMethod]
        public void ExportedFunctions_Should_Be_Exported()
        {
            var exportedFunctions = new List<ExportedFunction> { new ExportedFunction("print5", (env, args) => { printer.Print("5");
                return ObjectFactory.Null;
            }, (0, 0)) };
            settings = new ScriptSettings(exportedFunctions);
            settings.Functions.Should().HaveCount(1);
        }

        [TestMethod]
        public void Printer_Should_Print_With_Custom_Printer()
        {
            printer.Print("hej");
            printer.PrintedMessages.Should().BeEquivalentTo(new List<string> { "hej" });
        }
    }
}
