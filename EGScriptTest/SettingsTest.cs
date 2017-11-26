using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EGScript;
using EGScript.Objects;
using EGScript.Scripter;
using System.Collections.Generic;
using FluentAssertions;

namespace EGScriptTest
{
    [TestClass]
    public class SettingsTest
    {
        Settings settings;
        TestPrinter printer => settings.Printer as TestPrinter;

        private ScriptObject Print5(ScriptEnvironment env, List<ScriptObject> args)
        {
            printer.Print("5");
            return ObjectFactory.Null;
        }

        [TestInitialize]
        public void InitializeTest()
        {
            settings = new Settings { Printer = new TestPrinter() };
        }

        [TestMethod]
        public void ExportedFunctions_Should_Have_Count_Of_Zero_By_Default()
        {
            settings.ExportedFunctions.Should().HaveCount(0);
        }

        [TestMethod]
        public void ExportedFunctions_Should_Be_Exported()
        {
            var exportedFunctions = new List<ExportedFunction> { new ExportedFunction("print5", Print5, (0, 0)) };
            settings = new Settings { ExportedFunctions = exportedFunctions };
            settings.ExportedFunctions.Should().HaveCount(1);
        }

        [TestMethod]
        public void Printer_Should_Print_With_Custom_Printer()
        {
            printer.Print("hej");
            printer.PrintedMessages.Should().BeEquivalentTo(new List<string> { "hej" });
        }
    }
}
