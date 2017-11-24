using EGScript.Helpers;
using EGScript.Scripter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EGScriptTest
{
    [TestClass]
    public class ParserTest
    {
        Lexer lexer;
        [TestInitialize]
        public void TestInitializing()
        {
            var code = @"function testy()
{
    test = 10;
    return test;
}";
            lexer = new Lexer(code);
        }


        [TestMethod]
        public void ParseScript_Script_Should_Return_Complete_Abstract_Syntax_Tree()
        {
            var parser = new Parser(lexer, new FileHandler(""));
            var ast = parser.ParseScript();
        }
    }
}
