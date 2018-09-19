using System.Collections.Generic;
using EGScript.AbstractSyntaxTree;
using EGScript.Objects;
using EGScript.OperationCodes;

namespace EGScript.Scripter
{
    public class Compiler : IVisitor
    {
        private readonly ScriptEnvironment _environment;
        private const uint Break = 0xFAABFACE;
        private const uint Continue = 0xFAABBABE;
        private const uint BranchToCodeBlock = 0xFAABDAAD;
        public const uint ReferenceTableIndex = 0xDEADFAAB;
        private Stack<ASTFunctionCall> CalledFunctions { get; }

        private Compiler(ScriptEnvironment environment)
        {
            _environment = environment;
            CalledFunctions = new Stack<ASTFunctionCall>();
        }

        public static void Compile(ScriptEnvironment environment, AST ast)
        {
            var compiler = new Compiler(environment);
            compiler.Visit(ast);
        }

        public void Visit(AST ast)
        {
            for (int i = 0; i < ast.GlobalVars.Count; i++)
                ast.GlobalVars[i].Accept(this);

            for (int i = 0; i < ast.Classes.Count; i++)
                ast.Classes[i].Accept(this);

            for (int i = 0; i < ast.Functions.Count; i++)
                ast.Functions[i].Accept(this);

            VerifyThatCalledFunctionsExist();
        }

        private void VerifyThatCalledFunctionsExist()
        {
            for (int i = 0; i < CalledFunctions.Count; i++)
            {
                var func = CalledFunctions.Pop();
                var callFunction = _environment.FindFunction(func.Name);
                if (callFunction == null)
                    throw new CompilerException($"Function '{func.Name}' has not been defined.");
                if (func.Arguments.Count != callFunction.Arguments.Count)
                    throw new CompilerException($"Function '{func.Name}' expects {callFunction.Arguments.Count} arguments.");
            }
        }

        public void Visit(ASTClassDefinition _class)
        {
            var name = _class.Name;
            var baseName = _class.Base;

            Class newClass;

            if (baseName.Length == 0)
                newClass = ObjectFactory.Class(name);
            else
            {
                var baseClass = _environment.FindClass(baseName);
                if(baseClass == null)
                     throw new CompilerException($"Base class '{name}' has not been defined.");

                newClass = ObjectFactory.Class(name, baseClass);
            }

            _environment.AddClass(newClass);
            var memberDefinitions = _class.MemberDefinitions;

            for(int i = 0; i < memberDefinitions.Count; i++)
            {
                memberDefinitions[i].Accept(this, newClass);
            }
        }

        public void Visit(ASTMemberFunction functionDeclaration)
        {
            var _class = _environment.FindClass(functionDeclaration.BaseClass);

            if (_class == null)
                throw new CompilerException($"Class '{functionDeclaration.BaseClass}' does not exist.");

            var function = _class.FindFunction(functionDeclaration.Name);
            if (function == null)
                throw new CompilerException($"Class '{functionDeclaration.BaseClass}' does not define function '{functionDeclaration.Name}'.");

            for(int i = functionDeclaration.Arguments.Count - 1; i >= 0; i--)
            {
                function.Code.Write(OpCodeFactory.Set(ObjectFactory.String(functionDeclaration.Arguments[i])));
                function.Code.Write(OpCodeFactory.Pop);
            }

            if(functionDeclaration.Body != null)
            {
                functionDeclaration.Body.Accept(this, function);

                // We add this in case the user explicitly doesn't return anything in which case the function is void.
                function.Code.Write(OpCodeFactory.Push(ObjectFactory.Null));
                function.Code.Write(OpCodeFactory.Return);
            }
        }

        public void Visit(ASTGlobalFunction functionDeclaration)
        {
            var newFunction = ObjectFactory.Function(functionDeclaration.Name, functionDeclaration.Arguments);
            _environment.AddFunction(newFunction);

            for (int i = functionDeclaration.Arguments.Count - 1; i >= 0; i--)
            {
                newFunction.Code.Write(OpCodeFactory.Set(ObjectFactory.String(functionDeclaration.Arguments[i])));
                newFunction.Code.Write(OpCodeFactory.Pop);
            }

            if(functionDeclaration.Body != null)
            {
                functionDeclaration.Body.Accept(this, newFunction);

                // We add this in case the user explicitly doesn't return anything in which case the function is void.
                newFunction.Code.Write(OpCodeFactory.Push(ObjectFactory.Null));
                newFunction.Code.Write(OpCodeFactory.Return);
            }
        }

        public void Visit(ASTVariableDefinition variableDefinition, Class _class)
        {
            var scope = _class.Scope;
            scope.Define(variableDefinition.Name);
        }

        public void Visit(ASTFunctionDefinition functionDefinition, Class _class)
        {
            var func = ObjectFactory.Function(functionDefinition.Name, functionDefinition.Arguments);
            if (functionDefinition.Body != null)
            {
                functionDefinition.Body.Accept(this, func);

                // return null if script doesn't explicitly return something
                func.Code.Write(OpCodeFactory.Push(ObjectFactory.Null));
                func.Code.Write(OpCodeFactory.Return);
            }
            _class.AddFunction(func);
        }

        public void Visit(ASTBlock block, Function function)
        {
            List<ASTStatementBase> statements = block.Statements;
            for (int i = 0; i < statements.Count; i++)
                statements[i].Accept(this, function);
        }

        public void Visit(ASTIf astIf, Function function)
        {
            // Compile condition
            astIf.Condition.Accept(this, function);

            // If condition evaluates to false, branch past the if part
            function.Code.Write(OpCodeFactory.BranchIfFalse(0));
            var index = function.Code.Count - 1;

            // Compile if-part
            astIf.IfPart.Accept(this, function);

            // Set the offset after compiling the if-part
            function.Code[index].As<BranchIfFalse>().Argument = (uint)function.Code.Count;

            if (astIf.ElsePart != null)
                astIf.ElsePart.Accept(this, function);
        }

        public void Visit(ASTWhile whileStatement, Function function)
        {
            // do condition
            int condition = function.Code.Count;
            whileStatement.Condition.Accept(this, function);

            // branch out if condition evaluates to false
            function.Code.Write(OpCodeFactory.BranchIfFalse(0));
            int conditionBranch = function.Code.Count - 1;

            // compile body
            whileStatement.Body.Accept(this, function);

            // branch back to the condition
            function.Code.Write(OpCodeFactory.Branch((uint)condition));
            int end = function.Code.Count;

            // set the conditional branch offset
            function.Code[conditionBranch].As<BranchIfFalse>().Argument = (uint)end;

            // fill breaks/continues

            for(int i = condition; i < end; i++)
            {
                var instruction = function.Code[i];
                if (instruction is Branch breakBranch && breakBranch.Argument == Break) // break, jump to end
                    breakBranch.Argument = (uint)end;
                else if (instruction is Branch continueBranch && continueBranch.Argument == Continue) // continue, jump to condition
                    continueBranch.Argument = (uint)condition;
            }
        }

        public void Visit(ASTFor forStatement, Function function)
        {
            // initialize the loop variables
            int init = function.Code.Count;
            forStatement.Initializer.Accept(this, function);

            // do condition
            int condition = function.Code.Count;
            forStatement.Condition.Accept(this, function);

            // branch out if condition evaluates to false
            function.Code.Write(OpCodeFactory.BranchIfFalse(0));
            int conditionBranch = function.Code.Count - 1;

            // compile body
            forStatement.Body.Accept(this, function);

            // increment condition
            int increment = function.Code.Count;
            forStatement.Incrementer.Accept(this, function);

            // branch back to condition
            function.Code.Write(OpCodeFactory.Branch((uint)condition));
            int end = function.Code.Count;

            // set the conditional branch offset (branch if false)
            function.Code[conditionBranch].As<BranchIfFalse>().Argument = (uint)end;

            // fill breaks/continues
            for(int i = condition; i < end; i++)
            {
                var instruction = function.Code[i];
                if (instruction is Branch breakBranch && breakBranch.Argument == Break)
                    breakBranch.Argument = (uint)end;
                else if (instruction is Branch continueBranch && continueBranch.Argument == Continue)
                    continueBranch.Argument = (uint)increment;
            }
        }

        public void Visit(ASTSwitch switchStatement, Function function)
        {
            var switchObject = new StringObj("[-switch-]");

            // evaluate our first expression, set it to variable "[-switch-]"
            switchStatement.Expression.Accept(this, function);
            function.Code.Write(OpCodeFactory.Define(switchObject));

            // create our jump table
            int startOfJumpTable = function.Code.Count;
            bool defaultCase = false;
            for(int i = 0; i < switchStatement.Cases.Count; i++)
            {
                if (switchStatement.Cases[i].expression == null) // default case, should come last
                    defaultCase = true;
                else
                {
                    switchStatement.Cases[i].expression.Accept(this, function); // evaluate the comparison expression
                    function.Code.Write(OpCodeFactory.Reference(switchObject)); // reference our switch variable
                    function.Code.Write(OpCodeFactory.EqualsEquals); // compare them
                    function.Code.Write(OpCodeFactory.BranchIfTrue(BranchToCodeBlock)); // bramch if true to the actual code block
                }
            }

            int endOfJumpTable = function.Code.Count;
            function.Code.Write(OpCodeFactory.Branch(0));

            // write each case block
            int startBlocks = function.Code.Count;
            int lastBlock = 0;
            for(int i = 0; i < switchStatement.Cases.Count; i++)
            {
                // find the branch for this block (will be the first one) and write this blocks address
                for(int j = startOfJumpTable; j < endOfJumpTable; j++)
                {
                    var instruction = function.Code[j];
                    if(instruction is BranchIfTrue branchToBlock && branchToBlock.Argument == BranchToCodeBlock)
                    {
                        branchToBlock.Argument = (uint) function.Code.Count;
                        break;
                    }
                }

                if (switchStatement.Cases[i].statement == null) // fallthrough
                    continue;

                lastBlock = function.Code.Count;
                switchStatement.Cases[i].statement.Accept(this, function); // compile the block
            }
            int endBlocks = function.Code.Count;

            // set our final branch in case we had no default statement
            if (defaultCase)
                function.Code[endOfJumpTable].As<Branch>().Argument = (uint)lastBlock;
            else
                function.Code[endOfJumpTable].As<Branch>().Argument = (uint)endBlocks;

            // fill breaks
            for(int i = startBlocks; i < endBlocks; i++)
            {
                var instruction = function.Code[i];
                if (instruction is Branch breakBranch && breakBranch.Argument == Break) // break, jump to end
                    breakBranch.Argument = (uint)endBlocks;
            }
        }

        public void Visit(ASTBreak astBreak, Function function)
        {
            function.Code.Write(OpCodeFactory.Branch(Break));
        }

        public void Visit(ASTContinue astContinue, Function function)
        {
            function.Code.Write(OpCodeFactory.Branch(Continue));
        }

        public void Visit(ASTReturn returnStatement, Function function)
        {
            returnStatement.ReturnExpression.Accept(this, function);
            function.Code.Write(OpCodeFactory.Return);
        }

        public void Visit(ASTStatementExpression expressionStatement, Function function)
        {
            expressionStatement.Expression.Accept(this, function);
            function.Code.Write(OpCodeFactory.Pop);
        }

        public void Visit(ASTExpressionBase expression, Function function)
        {
            List<ASTExpressionBase> expressions = expression.Expressions;
            for(int i = 0; i < expressions.Count; i++)
            {
                expressions[i].Accept(this, function);
            }
        }

        public void Visit(ASTAssignment expression, Function function)
        {
            WriteAssignmentCode(expression, function);
        }

        private void WriteAssignmentCode(ASTAssignment expression, Function function)
        {
            switch (expression.OperationType)
            {
                case ASTAssignment.AssignmentType.ASSIGNMENT:
                    {
                        if (function.Scope.Find(expression.Variable) == null)
                        {
                            function.Scope.Define(expression.Variable, ObjectFactory.Null);
                        }
                        
                        expression.Expression.Accept(this, function);
                    }
                    break;
                case ASTAssignment.AssignmentType.ADDITION:
                    {
                        if (function.Scope.Find(expression.Variable) == null)
                        {
                            throw new CompilerException($"operator '+=' cannot be used with undefined variable '{expression.Variable}'.");
                        }

                        function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.Variable)));
                        expression.Expression.Accept(this, function);
                        function.Code.Write(OpCodeFactory.Add);
                    }
                    break;
                case ASTAssignment.AssignmentType.SUBTRACTION:
                    {
                        if (function.Scope.Find(expression.Variable) == null)
                        {
                            throw new CompilerException($"operator '-=' cannot be used with undefined variable '{expression.Variable}'.");
                        }

                        function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.Variable)));
                        expression.Expression.Accept(this, function);
                        function.Code.Write(OpCodeFactory.Subtract);
                    }
                    break;
                case ASTAssignment.AssignmentType.MULTIPLICATION:
                    {
                        if (function.Scope.Find(expression.Variable) == null)
                        {
                            throw new CompilerException($"operator '*=' cannot be used with undefined variable '{expression.Variable}'.");
                        }

                        function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.Variable)));
                        expression.Expression.Accept(this, function);
                        function.Code.Write(OpCodeFactory.Multiply);
                    }
                    break;
                case ASTAssignment.AssignmentType.DIVISION:
                    {
                        if (function.Scope.Find(expression.Variable) == null)
                        {
                            throw new CompilerException($"operator '/=' cannot be used with undefined variable '{expression.Variable}'.");
                        }

                        function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.Variable)));
                        expression.Expression.Accept(this, function);
                        function.Code.Write(OpCodeFactory.Divide);
                    }
                    break;
            }
            // write set
            if(expression is ASTMemberAssignmentInstance astmai)
                function.Code.Write(OpCodeFactory.SetMember(ObjectFactory.String(expression.Variable), ObjectFactory.String(astmai.InstanceName)));
            else if(expression is ASTMemberAssignment)
                function.Code.Write(OpCodeFactory.SetMember(ObjectFactory.String(expression.Variable), null));
            else
                function.Code.Write(OpCodeFactory.Set(ObjectFactory.String(expression.Variable)));
        }

        public void Visit(ASTCompare expression, Function function)
        {
            expression.Left.Accept(this, function);
            expression.Right.Accept(this, function);

            switch(expression.ComparisonType)
            {
                case ASTCompare.OperationType.OR: function.Code.Write(OpCodeFactory.Or); break;
                case ASTCompare.OperationType.AND: function.Code.Write(OpCodeFactory.And); break;
                case ASTCompare.OperationType.EQUALS_EQUALS: function.Code.Write(OpCodeFactory.EqualsEquals); break;
                case ASTCompare.OperationType.NOT_EQUALS: function.Code.Write(OpCodeFactory.NotEquals); break;
                case ASTCompare.OperationType.LESS_THAN: function.Code.Write(OpCodeFactory.LessThan); break;
                case ASTCompare.OperationType.GREATER_THAN: function.Code.Write(OpCodeFactory.GreaterThan); break;
                case ASTCompare.OperationType.LESS_THAN_EQUALS: function.Code.Write(OpCodeFactory.LessThanEquals); break;
                case ASTCompare.OperationType.GREATER_THAN_EQUALS: function.Code.Write(OpCodeFactory.GreaterThanEquals); break;
            }
        }

        public void Visit(ASTBinaryMathOperation expression, Function function)
        {
            expression.Left.Accept(this, function);
            expression.Right.Accept(this, function);

            switch(expression.MathOperationType)
            {
                case ASTBinaryMathOperation.OperationType.PLUS: function.Code.Write(OpCodeFactory.Add); break;
                case ASTBinaryMathOperation.OperationType.MINUS: function.Code.Write(OpCodeFactory.Subtract); break;
                case ASTBinaryMathOperation.OperationType.TIMES: function.Code.Write(OpCodeFactory.Multiply); break;
                case ASTBinaryMathOperation.OperationType.DIVIDE: function.Code.Write(OpCodeFactory.Divide); break;
            }
        }

        public void Visit(ASTUnaryMathOperation expression, Function function)
        {
            expression.Expression.Accept(this, function);

            switch(expression.MathOperationType)
            {
                case ASTUnaryMathOperation.OperationType.MINUS: function.Code.Write(OpCodeFactory.Negate); break;
                case ASTUnaryMathOperation.OperationType.NOT: function.Code.Write(OpCodeFactory.Not); break;
                case ASTUnaryMathOperation.OperationType.INCREMENT: function.Code.Write(OpCodeFactory.Increment); break;
                case ASTUnaryMathOperation.OperationType.DECREMENT: function.Code.Write(OpCodeFactory.Decrement); break;
            }
        }

        public void Visit(ASTNew expression, Function function)
        {
            var _class = _environment.FindClass(expression.Name);

            if (_class == null)
                throw new CompilerException($"Class '{expression.Name}' does not exist.");

            var constructor = _class.FindFunction(expression.Name);

            if (constructor == null)
                throw new CompilerException($"Constructor for class {expression.Name} does not exist.");

            if (expression.Arguments.Count != constructor.Arguments.Count)
                throw new CompilerException($"Function '{expression.Name}' expects {constructor.Arguments.Count} arguments.");

            for(int i = 0; i < expression.Arguments.Count; i++)
            {
                expression.Arguments[i].Accept(this, function);
            }

            var instance = ObjectFactory.Instance(_class);

            function.Code.Write(OpCodeFactory.Push(instance));
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(expression.Arguments.Count)));
            function.Code.Write(OpCodeFactory.MemberFunctionCall(ObjectFactory.String(expression.Name)));
            function.Code.Write(OpCodeFactory.Push(instance));
        }


        public void Visit(ASTGlobalFunctionCall expression, Function function)
        {
            var exportedFunction = _environment.FindExportedFunction(expression.Name);
            if(exportedFunction != null)
            {
                for(int i = 0; i < expression.Arguments.Count; i++)
                    expression.Arguments[i].Accept(this, function);

                function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(expression.Arguments.Count)));
                function.Code.Write(OpCodeFactory.ExportedFunctionCall(ObjectFactory.String(expression.Name)));
            }
            else
            {
                CalledFunctions.Push(expression);
                // Old way to check if a function exists (where functions have to be defined in a first->last order depending on their usage)
                //var callFunction = _environment.FindFunction(expression.Name);
                //if (callFunction == null)
                //    throw new CompilerException($"Function '{expression.Name}' has not been defined.");
                //if (expression.Arguments.Count != callFunction.Arguments.Count)
                //    throw new CompilerException($"Function '{expression.Name}' expects {callFunction.Arguments.Count} arguments.");
                for (int i = 0; i < expression.Arguments.Count; i++)
                    expression.Arguments[i].Accept(this, function);

                function.Code.Write(OpCodeFactory.FunctionCall(ObjectFactory.String(expression.Name)));
            }
        }

        public void Visit(ASTMemberFunctionCall expression, Function function)
        {
            for (int i = 0; i < expression.Arguments.Count; i++)
                expression.Arguments[i].Accept(this, function);

            function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.InstanceName)));
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(expression.Arguments.Count)));
            function.Code.Write(OpCodeFactory.MemberFunctionCall(ObjectFactory.String(expression.Name)));
        }

        public void Visit(ASTMemberAssignmentInstance expression, Function function)
        {
            WriteAssignmentCode(expression, function);
        }

        public void Visit(ASTMemberAssignment expression, Function function)
        {
            WriteAssignmentCode(expression, function);
        }

        public void Visit(ASTMemberAccess expression, Function function)
        {
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.String(expression.Name)));
            function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.InstanceName)));
            function.Code.Write(OpCodeFactory.MemberAccess());
        }

        public void Visit(ASTIdentifier expression, Function function)
        {
            var variable = function.Scope.Find(expression.Name);

            if(variable == null)
            {
                function.Scope.Define(expression.Name, ObjectFactory.Null);
                variable = function.Scope.Find(expression.Name);
            }

            function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.Name)));
        }

        public void Visit(ASTNumber number, Function function)
        {
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(number.Value)));
        }

        public void Visit(ASTString str, Function function)
        {
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.String(str.Text)));
        }

        public void Visit(ASTNull nul, Function function)
        {
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.Null));
        }

        public void Visit(ASTTrue expression, Function function)
        {
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.True));
        }

        public void Visit(ASTFalse expression, Function function)
        {
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.False));
        }

        public void Visit(ASTTable astTable, Function function)
        {
            for (int i = 0; i < astTable.IntegerValues.Count; i++)
                astTable.IntegerValues[i].Accept(this, function);
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(astTable.IntegerValues.Count))); // push number of integer value keys + values

            foreach (var stringVal in astTable.StringValues)
                stringVal.Value.Accept(this, function);
            function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(astTable.StringValues.Count))); // push number of string value keys + values

            function.Code.Write(OpCodeFactory.MakeTable); // make table out of keys and values
        }

        public void Visit(ASTTableGet astTableGet, Function function)
        {
            // if indexCount == 1, PUSH only the object at that index
            // if indexCount > 1, PUSH a new table object with those indexes? sound good? oki

            //astTableGet.Variable.Accept(this, function); // reference the actual table on top of stack
            var identifier = astTableGet.Variable as ASTIdentifier;
            var table = function.Scope.Find(identifier.Name) as Table;
            if (astTableGet.TableIndexes.Count == 1)
            {
                // PUSH object at the table index
                astTableGet.TableIndexes[0].Accept(this, function); // push the index of the item to return
                function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(1))); // push the amount of indexes we are getting
                function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(identifier.Name), ReferenceTableIndex));// reference the table
            }
            else
            {
                for (int i = astTableGet.TableIndexes.Count - 1; i >= 0;  i--) // push all indexes
                    astTableGet.TableIndexes[i].Accept(this, function);

                function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(astTableGet.TableIndexes.Count))); // push the amount of indexes
                function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(identifier.Name), ReferenceTableIndex)); // reference the table
            }
        }

        public void Visit(ASTTableElement astTableElement, Function function)
        {
            astTableElement.Value.Accept(this, function); // push value first
            if (astTableElement.Type == ExpressionType.STRING)
                function.Code.Write(OpCodeFactory.Push(ObjectFactory.String(astTableElement.StringKey))); // push string key if string key
            else if (astTableElement.Type == ExpressionType.NUMBER)
                function.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(astTableElement.IntKey))); // push number key if number key
        }

        public void Visit(ASTCount astCount, Function function)
        {
            function.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(astCount.Identifier)));
            function.Code.Write(OpCodeFactory.Count);
        }

        public void Visit(ASTGlobalVariableAssignment expression)
        {
            expression.Expression.Accept(this, _environment.Globals);
            _environment.Globals.Code.Write(OpCodeFactory.Global(ObjectFactory.String(expression.Variable)));

            // more fun way to do the same thing as in opcode 'Global':
            //if (_environment.Globals.Scope.Find(expression.Variable) == null)
            //{
            //    _environment.Globals.Scope.Define(expression.Variable, ObjectFactory.Null);
            //}
            //else
            //{
            //    // reference variable value
            //    _environment.Globals.Code.Write(OpCodeFactory.Reference(ObjectFactory.String(expression.Variable)));
            //    // evaluate assignment expression
            //    expression.Expression.Accept(this, _environment.Globals);
            //    // check if referenced value equals evaluated value
            //    _environment.Globals.Code.Write(OpCodeFactory.EqualsEquals);
            //    // branch if both values are equivalent
            //    var branchTrue = _environment.Globals.Code.Count;
            //    _environment.Globals.Code.Write(OpCodeFactory.BranchIfTrue(0));
            //    // push argument of exported function call
            //    _environment.Globals.Code.Write(OpCodeFactory.Push(ObjectFactory.String($"Initializing a global variable ('{expression.Variable}') with two different values is not possible.")));
            //    // push number of arguments for exported function call
            //    _environment.Globals.Code.Write(OpCodeFactory.Push(ObjectFactory.Number(1)));
            //    // call exported function
            //    _environment.Globals.Code.Write(OpCodeFactory.ExportedFunctionCall(ObjectFactory.String("error")));
            //    // branch to next line if true
            //    _environment.Globals.Code[branchTrue].As<BranchIfTrue>().Argument = (uint)_environment.Globals.Code.Count;
            //    return;
            //}
            //expression.Expression.Accept(this, _environment.Globals);
            //_environment.Globals.Code.Write(OpCodeFactory.Set(ObjectFactory.String(expression.Variable)));
        }
    }
}
