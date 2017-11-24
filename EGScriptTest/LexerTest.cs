using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EGScript.Scripter;
using FluentAssertions;

namespace EGScriptTest
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void NextToken_Should_Return_Last_Token_Before_Returning_END_OF_FILE()
        {
            var script = @"function testy()
{
    test = 10;
}";
            var lex = new Lexer(script);
            lex.NextToken().Type.Should().Be(TokenType.FUNCTION);
            lex.NextToken().Type.Should().Be(TokenType.IDENTIFIER);
            lex.NextToken().Type.Should().Be(TokenType.LEFT_PARENTHESIS);
            lex.NextToken().Type.Should().Be(TokenType.RIGHT_PARENTHESIS);
            lex.NextToken().Type.Should().Be(TokenType.LEFT_BRACE);
            lex.NextToken().Type.Should().Be(TokenType.IDENTIFIER);
            lex.NextToken().Type.Should().Be(TokenType.EQUALS);
            lex.NextToken().Type.Should().Be(TokenType.NUMBER);
            lex.NextToken().Type.Should().Be(TokenType.SEMICOLON);
            lex.NextToken().Type.Should().Be(TokenType.RIGHT_BRACE);
            lex.NextToken().Type.Should().Be(TokenType.END_OF_FILE);
        }

        [TestMethod]
        public void NextToken_Should_Return_END_OF_FILE_If_Script_Is_Empty()
        {
            var script = "";
            var lex = new Lexer(script);
            lex.NextToken().Type.Should().Be(TokenType.END_OF_FILE);
        }

        [TestMethod]
        public void NextToken_Should_Throw_LexerException_If_String_Is_Not_Terminated()
        {
            var script = @"str = ""hej";
            var lex = new Lexer(script);
            lex.NextToken(); // = 'str'
            lex.NextToken(); // = '='
            Action throwyAction = () => lex.NextToken(); // = '"hej'
            throwyAction.Should().Throw<LexerException>();
        }

        [TestMethod]
        public void NextToken_Should_Throw_LexerException_If_First_Part_Of_And_Operator_Is_Not_Followed_By_Last_Part()
        {
            var script = "&|";
            var lex = new Lexer(script);
            Action throwyAction = () => lex.NextToken();
            throwyAction.Should().Throw<LexerException>();
        }
    }
}
