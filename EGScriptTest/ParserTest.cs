using EGScript.Helpers;
using EGScript.Scripter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EGScriptTest
{
    [TestClass]
    public class ParserTest
    {
        private Lexer _lexer;
        [TestInitialize]
        public void TestInitializing()
        {
            var code = @"function testy()
{
    test = 10;
    return test;
}";
            _lexer = new Lexer(code);
        }


        [TestMethod]
        public void ParseScript_Script_Should_Return_Complete_Abstract_Syntax_Tree()
        {
            var parser = new Parser(_lexer, new FileHandler(""));
            var ast = parser.ParseScript();
            ast.Functions.Should().HaveCount(1);

        }
    }
}
