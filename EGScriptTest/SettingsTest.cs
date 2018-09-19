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
        private ScriptSettings _settings;
        private TestPrinter Printer => _settings.Printer as TestPrinter;

        [TestInitialize]
        public void InitializeTest()
        {
            _settings = new ScriptSettings(new TestPrinter());
        }

        [TestMethod]
        public void ExportedFunctions_Should_Have_Count_Of_Zero_By_Default()
        {
            _settings.Functions.Should().HaveCount(0);
        }

        [TestMethod]
        public void ExportedFunctions_Should_Be_Exported()
        {
            var exportedFunctions = new List<ExportedFunction> { new ExportedFunction("print5", (env, args) => { Printer.Print("5");
                return ObjectFactory.Null;
            }, (0, 0)) };
            _settings = new ScriptSettings(exportedFunctions);
            _settings.Functions.Should().HaveCount(1);
        }

        [TestMethod]
        public void Printer_Should_Print_With_Custom_Printer()
        {
            Printer.Print("hej");
            Printer.PrintedMessages.Should().BeEquivalentTo(new List<string> { "hej" });
        }
    }
}
