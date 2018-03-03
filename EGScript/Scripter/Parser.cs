using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EGScript.AbstractSyntaxTree;
using EGScript.Helpers;

namespace EGScript.Scripter
{
    public class Parser
    {
        private Lexer _lexer { get; }
        private Token[] _buffer = new Token[2] { new Token(), new Token() };
        private int _loopLevel;
        private int _switchLevel;
        private readonly IFileHandler _fileHandler;

        public Parser(Lexer lexer, IFileHandler fileHandler)
        {
            _lexer = lexer;
            _fileHandler = fileHandler;
            Consume();
            Consume();
        }

        public AST ParseScript()
        {
            List<ASTFunctionBase> functionDeclarations = new List<ASTFunctionBase>();
            List<ASTClassDefinition> classDeclarations = new List<ASTClassDefinition>();
            List<ASTGlobalVariableAssignment> globalVars = new List<ASTGlobalVariableAssignment>();

            while(_buffer[0].Type != TokenType.END_OF_FILE)
            {
                switch(_buffer[0].Type)
                {
                    case TokenType.INCLUDE:
                        Consume();
                        IncludeScript(classDeclarations, functionDeclarations, globalVars);
                        break;
                    case TokenType.GLOBAL:
                        Consume();
                        DeclareGlobalVariables(globalVars);
                        break;
                    case TokenType.CLASS:
                        Consume();
                        DeclareClass(classDeclarations);
                        break;
                    case TokenType.FUNCTION:
                        Consume();
                        DeclareFunction(functionDeclarations);
                        break;
                    case TokenType.SEMICOLON:
                        Consume();
                        break;
                    default:
                        // no main function defined, wrap a main function around the script
                        WrapMainFunctionAroundScript(functionDeclarations);
                        break;
                        //throw new ParserException("Class definition or function declaration expected.", _lexer.Line, _lexer.Character);
                        // figure out a good way to handle this situation /\
                        // should an exception be thrown or should this be accepted? (no main() function)
                        // should it be possible to toggle this behaviour?
                }
            }
            // TODO: Finish implementing classes
            return new AST(classDeclarations, functionDeclarations, globalVars);
        }

        private void DeclareGlobalVariables(List<ASTGlobalVariableAssignment> globalVars)
        {
            Require(TokenType.LEFT_BRACE);

            while (!Match(TokenType.RIGHT_BRACE))
            {
                var assignment = ParseExpression1() as ASTAssignment;
                if (assignment == null)
                    throw new ParserException($"Expected variable assignment.", _lexer.Line, _lexer.Character);
                Require(TokenType.SEMICOLON);

                globalVars.Add(assignment.ToGlobalVariable());
            }

            Require(TokenType.RIGHT_BRACE);
        }

        private void IncludeScript(List<ASTClassDefinition> classDeclarations, List<ASTFunctionBase> functionDeclarations, List<ASTGlobalVariableAssignment> globalVars)
        {
            // include "script.soup";
            // get script name
            var name = Require(TokenType.STRING).Text;
            Require(TokenType.SEMICOLON);
            name = !Path.IsPathRooted(name) ? Path.Combine(_fileHandler.WorkingDirectory, name) : name;
            var parser = new Parser(new Lexer(_fileHandler.ReadFileToEnd(name)), _fileHandler.Copy(Path.GetDirectoryName(name)));
            var includedScript = parser.ParseScript();

            for(int i = 0; i < includedScript.Classes.Count; i++)
            {
                classDeclarations.Add(includedScript.Classes[i]);
            }

            for(int i = 0; i < includedScript.Functions.Count; i++)
            {
                functionDeclarations.Add(includedScript.Functions[i]);
            }

            for(int i = 0; i < includedScript.GlobalVars.Count; i++)
            {
                globalVars.Add(includedScript.GlobalVars[i]);
            }
        }

        private void DeclareClass(List<ASTClassDefinition> classDeclarations)
        {
            // class *identifier*
            var name = Require(TokenType.IDENTIFIER).Text;
            var baseClass = "";

            if (Match(TokenType.COLON))
            {
                // class *identifier* : *identifier*
                Consume();
                baseClass = Require(TokenType.IDENTIFIER).Text;
            }

            Require(TokenType.LEFT_BRACE);

            List<ASTMemberDefinition> memberDefinitions = new List<ASTMemberDefinition>();

            while(!Match(TokenType.RIGHT_BRACE))
            {
                DeclareMemberDefinition(memberDefinitions);
            }

            Require(TokenType.RIGHT_BRACE);

            if(baseClass.Length != 0)
            {
                classDeclarations.Add(new ASTClassDefinition(name, baseClass, memberDefinitions));
            }
            else
            {
                classDeclarations.Add(new ASTClassDefinition(name, memberDefinitions));
            }
        }

        private void DeclareMemberDefinition(List<ASTMemberDefinition> memberDefinitions)
        {
            //var name = Require(TokenType.IDENTIFIER).Text;
            if(Match(TokenType.FUNCTION))
            {
                Consume();
                var name = Require(TokenType.IDENTIFIER).Text;
                Require(TokenType.LEFT_PARENTHESIS);
                var arguments = new List<string>();

                while(!Match(TokenType.RIGHT_PARENTHESIS))
                {
                    arguments.Add(Require(TokenType.IDENTIFIER).Text);

                    if(Match(TokenType.COMMA))
                    {
                        Consume();
                        if(!Match(TokenType.IDENTIFIER))
                            throw new ParserException("Argument name expected after comma.", _lexer.Line, _lexer.Character);
                    }
                }

                Require(TokenType.RIGHT_PARENTHESIS);

                var body = ParseBlock();

                memberDefinitions.Add(new ASTFunctionDefinition(name, arguments, body));
            }
            //else
            //{
            //    //var variable = ParseExpression();
            //    //if (variable is ASTAssignment assignment && assignment.Type == ExpressionType.ASSIGNMENT)
            //    //{
            //    //    memberDefinitions.Add(new ASTAssignedVariableDefinition(assignment.Variable, assignment.Expression));
            //    //}
            //    //else if (variable is ASTIdentifier identifier)
            //    //{
            //    //    memberDefinitions.Add(new ASTVariableDefinition(identifier.Name));
            //    //}
            //    //else
            //    //    throw new ParserException($"Variable definition expected.", _lexer.Line, _lexer.Character);
            //}
            else
            {
                var name = Require(TokenType.IDENTIFIER).Text;
                // declare class variable
                memberDefinitions.Add(new ASTVariableDefinition(name));

                while (Match(TokenType.COMMA))
                {
                    Consume();
                    name = Require(TokenType.IDENTIFIER).Text;
                    memberDefinitions.Add(new ASTVariableDefinition(name));
                }

                Require(TokenType.SEMICOLON);
            }
        }

        private void DeclareFunction(List<ASTFunctionBase> functionDeclarations)
        {
            var name = Require(TokenType.IDENTIFIER).Text;
            var baseClass = "";

            if(Match(TokenType.DOUBLE_COLON))
            {
                Consume();

                baseClass = Require(TokenType.IDENTIFIER).Text;
                var tmp = baseClass;
                baseClass = name;
                name = tmp;
            }

            Require(TokenType.LEFT_PARENTHESIS);
            List<string> arguments = new List<string>();

            while(!Match(TokenType.RIGHT_PARENTHESIS))
            {
                arguments.Add(Require(TokenType.IDENTIFIER).Text);

                if(Match(TokenType.COMMA))
                {
                    Consume();

                    if (!Match(TokenType.IDENTIFIER))
                        throw new ParserException($"Argument name is expected following a comma.", _lexer.Line, _lexer.Character);
                }

            }

            Require(TokenType.RIGHT_PARENTHESIS);

            ASTStatementBase body = ParseBlock();

            if (baseClass.Length != 0)
                functionDeclarations.Add(new ASTMemberFunction(name, baseClass, arguments, body));
            else
                functionDeclarations.Add(new ASTGlobalFunction(name, arguments, body));
        }

        
        private void WrapMainFunctionAroundScript(List<ASTFunctionBase> functionDeclarations)
        {
            List<ASTStatementBase> block = new List<ASTStatementBase>();
            while (!Match(TokenType.END_OF_FILE))
            {
                block.Add(ParseStatement());
            }
            functionDeclarations.Add(new ASTGlobalFunction("main", new List<string>(), new ASTBlock(block)));
        }

        private ASTStatementBase ParseStatement()
        {
            switch(_buffer[0].Type)
            {
                case TokenType.LEFT_BRACE: return ParseBlock();
                case TokenType.IF: return ParseIf();
                case TokenType.WHILE: return ParseWhile();
                case TokenType.FOR: return ParseFor();
                case TokenType.SWITCH: return ParseSwitch();
                case TokenType.BREAK: return ParseBreak();
                case TokenType.CONTINUE: return ParseContinue();
                case TokenType.RETURN: return ParseReturn();

                default:
                    {
                        ASTExpressionBase expression = ParseExpression();
                        Require(TokenType.SEMICOLON);

                        return new ASTStatementExpression(expression);
                    }
            }
        }


        private ASTStatementBase ParseBlock()
        {
            Require(TokenType.LEFT_BRACE);

            List<ASTStatementBase> block = new List<ASTStatementBase>();

            while (!Match(TokenType.RIGHT_BRACE))
            {
                if (Match(TokenType.END_OF_FILE))
                    throw new ParserException("Right brace expected", _lexer.Line, _lexer.Character);

                block.Add(ParseStatement());
            }

            Require(TokenType.RIGHT_BRACE);

            return new ASTBlock(block);

        }

        private ASTStatementBase ParseIf()
        {
            Require(TokenType.IF);
            Require(TokenType.LEFT_PARENTHESIS);

            ASTExpressionBase condition = ParseExpression();

            Require(TokenType.RIGHT_PARENTHESIS);

            ASTStatementBase ifPart = ParseStatement();
            ASTStatementBase elsePart = null;

            if(Match(TokenType.ELSE))
            {
                Consume();
                elsePart = ParseStatement();
            }

            if (elsePart == null)
                return new ASTIf(condition, ifPart);
            return new ASTIf(condition, ifPart, elsePart);
        }


        private ASTStatementBase ParseWhile()
        {
            Require(TokenType.WHILE);
            Require(TokenType.LEFT_PARENTHESIS);

            ASTExpressionBase condition = ParseExpression();

            Require(TokenType.RIGHT_PARENTHESIS);

            _loopLevel++;
            ASTStatementBase body = ParseStatement();
            _loopLevel--;

            return new ASTWhile(condition, body);
        }

        private ASTStatementBase ParseFor()
        {
            Require(TokenType.FOR);
            Require(TokenType.LEFT_PARENTHESIS);

            ASTExpressionBase initializer = ParseExpression();
            Require(TokenType.SEMICOLON);

            ASTExpressionBase condition = ParseExpression();
            Require(TokenType.SEMICOLON);

            ASTExpressionBase incrementer = ParseExpression();
            Require(TokenType.RIGHT_PARENTHESIS);

            _loopLevel++;
            ASTStatementBase body = ParseStatement();
            _loopLevel--;

            return new ASTFor(initializer, condition, incrementer, body);
        }

        private ASTStatementBase ParseSwitch()
        {
            Require(TokenType.SWITCH);

            if (_switchLevel > 0)
                throw new ParserException("Nested switch statements are currently not supported.", _lexer.Line, _lexer.Character);

            Require(TokenType.LEFT_PARENTHESIS);

            ASTExpressionBase check = ParseExpression();

            Require(TokenType.RIGHT_PARENTHESIS);

            Require(TokenType.LEFT_BRACE);

            ASTSwitch switchStatement = new ASTSwitch(check);

            while(!Match(TokenType.RIGHT_BRACE))
            {
                if (Match(TokenType.END_OF_FILE))
                    throw new ParserException("'}' expected!", _lexer.Line, _lexer.Character);

                if (Match(TokenType.DEFAULT))
                {
                    Require(TokenType.DEFAULT);
                    Require(TokenType.COLON);
                    _switchLevel++;
                    switchStatement.AddCase(null, ParseStatement());
                    _switchLevel--;
                    if (!Match(TokenType.RIGHT_BRACE))
                        throw new ParserException("A default case must be the final case of a switch statement.", _lexer.Line, _lexer.Character);
                    break;
                }

                Require(TokenType.CASE);
                ASTExpressionBase _case = ParseExpression();
                if (_case == null)
                    throw new ParserException("Identifier expected.", _lexer.Line, _lexer.Character);

                Require(TokenType.COLON);

                if(Match(TokenType.CASE) || Match(TokenType.DEFAULT))
                {
                    switchStatement.AddCase(_case, null);
                }
                else
                {
                    _switchLevel++;
                    switchStatement.AddCase(_case, ParseStatement());
                    _switchLevel--;
                }
                /* for now, I don't think forcing the scripter to use braces to have a "break;" after a case is necessary, eg. in the situation of
                 * case 5:
                 * {
                 *  print("hello");
                 *  break;
                 * }
                 * 
                 * you might as well just type
                 * case 5:
                 *  print("hello");
                 *  break;
                 */
                if (Match(TokenType.BREAK)) // so let's allow that
                {
                    Consume(); // consume 'break'
                    Require(TokenType.SEMICOLON); // require ';'
                    var currentCase = switchStatement.Cases[switchStatement.Cases.Count - 1]; // get statement of current case
                    // and add the 'break' to the end of it
                    switchStatement.Cases[switchStatement.Cases.Count - 1] = (currentCase.expression,
                        new ASTBlock(new List<ASTStatementBase>
                        {
                            currentCase.statement, new ASTBreak()
                        }));
                }
            }

            Require(TokenType.RIGHT_BRACE);

            if (switchStatement.Cases.Count == 0)
                throw new ParserException("Empty switch statements are not allowed.", _lexer.Line, _lexer.Character);

            return switchStatement;
        }

        private ASTStatementBase ParseBreak()
        {
            Require(TokenType.BREAK);
            Require(TokenType.SEMICOLON);

            if (_loopLevel == 0 && _switchLevel == 0)
                throw new ParserException("break is only allowed within loop or switch construct.", _lexer.Line, _lexer.Character);

            return new ASTBreak();
        }

        private ASTStatementBase ParseContinue()
        {
            Require(TokenType.CONTINUE);
            Require(TokenType.SEMICOLON);

            if (_loopLevel == 0)
                throw new ParserException("continue is only allowed within loop construct.", _lexer.Line, _lexer.Character);

            return new ASTContinue();
        }

        private ASTStatementBase ParseReturn()
        {
            Require(TokenType.RETURN);
            ASTExpressionBase returnExpression = ParseExpression();
            Require(TokenType.SEMICOLON);

            return new ASTReturn(returnExpression);
        }

        /// <summary>
        /// Handles *expression* "," *expression*
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression()
        {
            ASTExpressionBase expression = ParseExpression1();
            List<ASTExpressionBase> expressionList = new List<ASTExpressionBase>();
            expressionList.Add(expression);

            while(Match(TokenType.COMMA))
            {
                expressionList.Add(ParseExpression1());
            }

            return new ASTExpressionBase(expressionList);
        }

        /// <summary>
        /// Handles *leftValue* "=" *expression* (assignment)
        /// *variable* "+=" *expression*
        /// *variable* "-=" *expression*
        /// *variable* *= *expression*
        /// *variable* /= *expression*
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression1()
        {
            ASTExpressionBase expression = ParseExpression2();

            while (Match(TokenType.EQUALS) || Match(TokenType.PLUS_EQUALS) || Match(TokenType.MINUS_EQUALS) || Match(TokenType.TIMES_EQUALS) || Match(TokenType.DIVIDE_EQUALS))
            {
                if (expression.Type != ExpressionType.IDENTIFIER)
                    throw new ParserException("left side of an assignment must be a variable.", _lexer.Line, _lexer.Character);
                switch (_buffer[0].Type)
                {
                    case TokenType.EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression1();
                            expression = new ASTAssignment(ASTAssignment.AssignmentType.ASSIGNMENT, (expression as ASTIdentifier).Name, expressionRight);
                        }
                        break;
                    case TokenType.PLUS_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression1();
                            expression = new ASTAssignment(ASTAssignment.AssignmentType.ADDITION, (expression as ASTIdentifier).Name, expressionRight);
                        }
                        break;
                    case TokenType.MINUS_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression1();
                            expression = new ASTAssignment(ASTAssignment.AssignmentType.SUBTRACTION, (expression as ASTIdentifier).Name, expressionRight);
                        }
                        break;
                    case TokenType.TIMES_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression1();
                            expression = new ASTAssignment(ASTAssignment.AssignmentType.MULTIPLICATION, (expression as ASTIdentifier).Name, expressionRight);
                        }
                        break;
                    case TokenType.DIVIDE_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression1();
                            expression = new ASTAssignment(ASTAssignment.AssignmentType.DIVISION, (expression as ASTIdentifier).Name, expressionRight);
                        }
                        break;
                        
                }
            }

            return expression;
        }

        /// <summary>
        /// Handles *expression* "||" *expression* (or)
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression2()
        {
            ASTExpressionBase expression = ParseExpression3();

            while (Match(TokenType.OR))
            {
                Consume();
                ASTExpressionBase expressionRight = ParseExpression3();

                expression = new ASTCompare(ASTCompare.OperationType.OR, expression, expressionRight);
            }

            return expression;
        }

        /// <summary>
        /// Handles *expression* "&&" *expression* (and)
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression3()
        {
            ASTExpressionBase expression = ParseExpression4();
            while(Match(TokenType.AND))
            {
                Consume();
                ASTExpressionBase expressionRight = ParseExpression4();
                expression = new ASTCompare(ASTCompare.OperationType.AND, expression, expressionRight);
            }

            return expression;
        }

        /// <summary>
        /// <para>Handles *expression* "==" *expression</para>
        /// <para>*expression* "!=" *expression*</para>
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression4()
        {
            ASTExpressionBase expression = ParseExpression5();

            while(Match(TokenType.EQUALS_EQUALS) || Match(TokenType.NOT_EQUALS))
            {
                switch (_buffer[0].Type)
                {
                    case TokenType.EQUALS_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression5();
                            expression = new ASTCompare(ASTCompare.OperationType.EQUALS_EQUALS, expression, expressionRight);
                        }
                        break;
                    case TokenType.NOT_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression5();
                            expression = new ASTCompare(ASTCompare.OperationType.NOT_EQUALS, expression, expressionRight);
                        }
                        break;
                }
            }

            return expression;
        }

        /// <summary>
        /// <para>Handles *expression* "&lt;" *expression*</para>
        /// <para>*expression* "&gt;" *expression*</para>
        /// <para>*expression* "&lt;=" *expression*</para>
        /// <para>*expression* "&gt;=" *expression*</para>
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression5()
        {
            ASTExpressionBase expression = ParseExpression6();

            while(Match(TokenType.LESS_THAN) || Match(TokenType.GREATER_THAN) || Match(TokenType.LESS_THAN_EQUALS) || Match(TokenType.GREATER_THAN_EQUALS))
            {
                switch(_buffer[0].Type)
                {
                    case TokenType.LESS_THAN:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression6();
                            expression = new ASTCompare(ASTCompare.OperationType.LESS_THAN, expression, expressionRight);
                        }
                        break;
                    case TokenType.GREATER_THAN:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression6();
                            expression = new ASTCompare(ASTCompare.OperationType.GREATER_THAN, expression, expressionRight);
                        }
                        break;
                    case TokenType.LESS_THAN_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression6();
                            expression = new ASTCompare(ASTCompare.OperationType.LESS_THAN_EQUALS, expression, expressionRight);
                        }
                        break;
                    case TokenType.GREATER_THAN_EQUALS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression6();
                            expression = new ASTCompare(ASTCompare.OperationType.GREATER_THAN_EQUALS, expression, expressionRight);
                        }
                        break;
                }
            }

            return expression;
        }

        /// <summary>
        /// <para>Handles *expression* "+" *expression*</para>
        /// <para>*expression* "-" *expression*</para>
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression6()
        {
            ASTExpressionBase expression = ParseExpression7();
            
            while(Match(TokenType.PLUS) || Match(TokenType.MINUS))
            {
                switch (_buffer[0].Type)
                {
                    case TokenType.PLUS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression7();
                            expression = new ASTBinaryMathOperation(ASTBinaryMathOperation.OperationType.PLUS, expression, expressionRight);
                        }
                        break;
                    case TokenType.MINUS:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression7();
                            expression = new ASTBinaryMathOperation(ASTBinaryMathOperation.OperationType.MINUS, expression, expressionRight);
                        }
                        break;
                }
            }

            return expression;
        }

        /// <summary>
        /// <para>Handles *expression* "*" *expression*</para>
        /// <para>*expression* "/" *expression*</para>
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression7()
        {
            ASTExpressionBase expression = ParseExpression8();

            while (Match(TokenType.TIMES) || Match(TokenType.DIVIDE))
            {
                switch (_buffer[0].Type)
                {
                    case TokenType.TIMES:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression8();
                            expression = new ASTBinaryMathOperation(ASTBinaryMathOperation.OperationType.TIMES, expression, expressionRight);
                        }
                        break;
                    case TokenType.DIVIDE:
                        {
                            Consume();
                            ASTExpressionBase expressionRight = ParseExpression8();
                            expression = new ASTBinaryMathOperation(ASTBinaryMathOperation.OperationType.DIVIDE, expression, expressionRight);
                        }
                        break;
                }
            }

            return expression;
        }

        /// <summary>
        /// <para>Handles "-" *expression*</para>
        /// <para>"!" *expression*</para>
        /// <para>"new" *identifier* "(" *argumentList* ")"</para>
        /// <para>*identifier*++</para>
        /// <para>*identifier*--</para>
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression8()
        {
            ASTExpressionBase expression;

            switch(_buffer[0].Type)
            {
                case TokenType.IDENTIFIER:
                    {
                        expression = ParseExpression9();
                        if(Match(TokenType.INCREMENT) || Match(TokenType.DECREMENT))
                        {
                            ASTUnaryMathOperation.OperationType opType = Match(TokenType.INCREMENT) ? ASTUnaryMathOperation.OperationType.INCREMENT : ASTUnaryMathOperation.OperationType.DECREMENT;
                            expression = new ASTUnaryMathOperation(opType, expression);
                            Consume();
                        }
                    }
                    break;
                case TokenType.MINUS:
                    {
                        Consume();
                        expression = ParseExpression9();
                        expression = new ASTUnaryMathOperation(ASTUnaryMathOperation.OperationType.MINUS, expression);
                    }
                    break;
                case TokenType.NOT:
                    {
                        Consume();
                        expression = ParseExpression9();
                        expression = new ASTUnaryMathOperation(ASTUnaryMathOperation.OperationType.NOT, expression);
                    }
                    break;
                case TokenType.NEW:
                    {
                        Consume();
                        var name = Require(TokenType.IDENTIFIER).Text;
                        List<ASTExpressionBase> arguments = new List<ASTExpressionBase>();

                        Require(TokenType.LEFT_PARENTHESIS);

                        while(!Match(TokenType.RIGHT_PARENTHESIS))
                        {
                            arguments.Add(ParseExpression1());

                            if (Match(TokenType.COMMA))
                                Consume();
                        }

                        Require(TokenType.RIGHT_PARENTHESIS);

                        expression = new ASTNew(name, arguments);
                    }
                    break;
                default:
                    {
                        expression = ParseExpression9();
                    }
                    break;
            }

            return expression;
        }

        /// <summary>
        /// <para>Handles *identifier* "[" *tableKey* "]"</para>
        /// <para>*identifier* "(" *argumentList* ")"</para>
        /// <para>*identifier* -> *identifier* "(" *argumentList* ")"</para>
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression9()
        {
            ASTExpressionBase expression = ParseExpression10();

            while(Match(TokenType.LEFT_PARENTHESIS) || Match(TokenType.ARROW) || Match(TokenType.LEFT_BRACKET))
            {
                switch(_buffer[0].Type)
                {
                    case TokenType.LEFT_BRACKET:
                        {
                            Consume();
                            if (expression.Type != ExpressionType.IDENTIFIER)
                                throw new ParserException($"Table expected.", _lexer.Line, _lexer.Character);

                            var indexes = new List<ASTExpressionBase>();
                            while(!Match(TokenType.RIGHT_BRACKET))
                            {
                                indexes.Add(ParseExpression1());

                                if (Match(TokenType.COMMA))
                                    Consume();
                            }
                            Require(TokenType.RIGHT_BRACKET);
                            expression = new ASTTableGet(expression, indexes);
                        }
                        break;
                    case TokenType.LEFT_PARENTHESIS:
                        {
                            Consume();
                            if (expression.Type != ExpressionType.IDENTIFIER)
                                throw new ParserException($"Function call expected.", _lexer.Line, _lexer.Character);

                            List<ASTExpressionBase> arguments = new List<ASTExpressionBase>();

                            while(!Match(TokenType.RIGHT_PARENTHESIS))
                            {
                                arguments.Add(ParseExpression1());

                                if (Match(TokenType.COMMA))
                                    Consume();
                            }

                            Require(TokenType.RIGHT_PARENTHESIS);

                            expression = new ASTGlobalFunctionCall((expression as ASTIdentifier).Name, arguments);
                        }
                        break;
                    case TokenType.ARROW:
                        {
                            Consume();
                            if(expression.Type != ExpressionType.IDENTIFIER)
                                throw new ParserException($"class object expected.", _lexer.Line, _lexer.Character);

                            string memberName = Require(TokenType.IDENTIFIER).Text;

                            if(!Match(TokenType.LEFT_PARENTHESIS))
                            {
                                // return class variable
                                expression = new ASTIdentifier(memberName);
                                break;
                            }

                            Require(TokenType.LEFT_PARENTHESIS);
                            List<ASTExpressionBase> arguments = new List<ASTExpressionBase>();

                            while(!Match(TokenType.RIGHT_PARENTHESIS))
                            {
                                arguments.Add(ParseExpression1());

                                if (Match(TokenType.COMMA))
                                    Consume();
                            }

                            Require(TokenType.RIGHT_PARENTHESIS);

                            expression = new ASTMemberFunctionCall(memberName, (expression as ASTIdentifier).Name, arguments);
                        }
                        break;
                }
            }

            return expression;
        }

        /// <summary>
        /// <para>Handles "{" *key = value* list "}"</para>
        /// <para>"(" *expression* ")"</para>
        /// <para>*identifier*</para>
        /// <para>*number*</para>
        /// <para>*string*</para>
        /// <para>"null"</para>
        /// <para>"true"</para>
        /// <para>"false"</para>
        /// </summary>
        /// <returns></returns>
        private ASTExpressionBase ParseExpression10()
        {
            switch(_buffer[0].Type)
            {
                case TokenType.LEFT_BRACE:
                    {
                        Consume();

                        ASTTable table = new ASTTable();
                        while(!Match(TokenType.RIGHT_BRACE))
                        {
                            bool good = true;
                            if(Match(TokenType.LEFT_BRACKET)) // [key] = value
                            {
                                Consume();
                                if (!Match(TokenType.NUMBER) && !Match(TokenType.STRING))
                                    throw new ParserException("Expected string or number key for table.", _lexer.Line, _lexer.Character);
                                ASTExpressionBase key = ParseExpression10();
                                Require(TokenType.RIGHT_BRACKET);

                                Require(TokenType.EQUALS);

                                ASTExpressionBase value = ParseExpression2();
                                good = table.InsertValue(key, value);
                            }
                            else
                            {
                                ASTExpressionBase key = ParseExpression2();
                                if(key.Type == ExpressionType.IDENTIFIER) // key = expvalue || value
                                {
                                    if (Match(TokenType.EQUALS))
                                    {
                                        Require(TokenType.EQUALS);
                                        ASTExpressionBase value = ParseExpression2();
                                        good = table.InsertValue(key, value);
                                    }
                                    else // expvalue
                                        good = table.InsertValue(null, key);
                                }
                                else // value (no key)
                                {
                                    good = table.InsertValue(null, key);
                                }
                            }

                            if (!good)
                                throw new ParserException("Invalid key/value pair for table (possible duplicate?).", _lexer.Line, _lexer.Character);

                            if (Match(TokenType.COMMA))
                                Consume();
                        }

                        table.NormalizeValues();
                        Require(TokenType.RIGHT_BRACE);
                        return table;
                    }
                case TokenType.LEFT_PARENTHESIS:
                    {
                        Consume();
                        ASTExpressionBase expression = ParseExpression1();
                        Require(TokenType.RIGHT_PARENTHESIS);
                        return expression;
                    }
                case TokenType.IDENTIFIER:
                    {
                        var variable = Require(TokenType.IDENTIFIER).Text;
                        return new ASTIdentifier(variable);
                    }
                case TokenType.NUMBER:
                    {
                        double num = Require(TokenType.NUMBER).Number;
                        return new ASTNumber(num);
                    }
                case TokenType.STRING:
                    {
                        var str = Require(TokenType.STRING).Text;
                        return new ASTString(str);
                    }
                case TokenType.NULL:
                    {
                        Consume();
                        return new ASTNull();
                    }
                case TokenType.TRUE:
                    {
                        Consume();
                        return new ASTTrue();
                    }
                case TokenType.FALSE:
                    {
                        Consume();
                        return new ASTFalse();
                    }
                case TokenType.COUNT:
                    {
                        Consume();
                        var identifier = Require(TokenType.IDENTIFIER).Text;
                        return new ASTCount(identifier);

                    }
                default:
                    throw new ParserException("Primary expression expected", _lexer.Line, _lexer.Character);
                    

            }
        }

        private Token Consume()
        {
            Token consumedToken = _buffer[0];
            _buffer[0] = _buffer[1];
            _buffer[1] = _lexer.NextToken();
            return consumedToken;
        }

        private bool Match(TokenType type)
        {
            return _buffer[0].Type == type;
        }

        private Token Require(TokenType type)
        {
            if (_buffer[0].Type != type)
                throw new ParserException($"Unexpected token '{Token.TypeToString(_buffer[0].Type)}', expected '{Token.TypeToString(type)}'.", _lexer.Line, _lexer.Character);
            return Consume();
        }

        [Obsolete("Not sure if this should ever be used!")]
        private Token Require(params TokenType[] allowedTokenTypes)
        {
            if (!allowedTokenTypes.Contains(_buffer[0].Type))
                throw new ParserException($"Unexpected token '{Token.TypeToString(_buffer[0].Type)}', expected any of '{string.Join(", ", allowedTokenTypes.Select(_ => Token.TypeToString(_)))}'.", _lexer.Line, _lexer.Character);
            return Consume();

        }
    }
}
