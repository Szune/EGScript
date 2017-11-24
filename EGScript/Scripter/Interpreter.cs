using EGScript.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class Interpreter
    {
        private class CallFrame
        {
            public int Address;
            public Instance Instance;
            public Function Function;

            public CallFrame(Function func)
            {
                Address = 0;
                Function = func;
            }

            public CallFrame(Function func, Instance instance) : this(func)
            {
                Instance = instance;
            }
        }

        private ScriptEnvironment _environment;
        private Stack<CallFrame> _frames;
        private Stack<ScriptObject> _stack;
        private Stack<Scope> _scopes;

        public Interpreter(ScriptEnvironment environment)
        {
            _frames = new Stack<CallFrame>();
            _stack = new Stack<ScriptObject>();
            _scopes = new Stack<Scope>();
            _environment = environment;
        }

        public ScriptObject Execute()
        {
            var func = _environment.FindFunction("main");

            if (func == null)
                throw new InterpreterException("function main() does not exist.");

            _frames.Push(new CallFrame(func));
            _scopes.Push(func.Scope);

            while(_frames.Count > 0)
            {
                var frame = _frames.Peek();
                var instruction = frame.Function.Code[frame.Address];

                frame.Address++;

                switch(instruction.OpCode)
                {
                    case OperationCode.PUSH: _stack.Push(instruction.Object); break;
                    case OperationCode.POP: _stack.Pop(); break;
                    case OperationCode.NULL: _stack.Push(ScriptEnvironment.NullObject); break;

                    case OperationCode.CALL:
                        {
                            if (!instruction.Object.TryGetString(out StringObj s))
                                throw new InterpreterException($"Instruction object was of type '{instruction.Object.TypeName}', expected 'string'.");

                            var callFunction = _environment.FindFunction(s.Text);

                            if (callFunction == null)
                                throw new InterpreterException($"function '{s.Text}' does not exist.");

                            _frames.Push(new CallFrame(callFunction));
                            callFunction.Scope.Reset();
                            _scopes.Push(new Scope(callFunction.Scope)); // TODO: Figure out if this is supposed to be using Scope.Copy() instead
                        }
                        break;
                    case OperationCode.MCALL:
                        {
                            if (!_stack.Peek().TryGetNumber(out Number n))
                                throw new InterpreterException($"Object on top of stack was of type '{_stack.Peek().TypeName}', expected 'number'.");

                            double numArgs = n.Value;
                            _stack.Pop();

                            var instance = _stack.Peek();
                            //_stack.Pop(); <- why is this part commented out in the original C++ code?

                            var t = instance.Type;
                            if (t != ObjectType.INSTANCE)
                                throw new InterpreterException($"Member call expected class instance.");

                            var _class = (instance as Instance).Class;
                            if (!instruction.Object.TryGetString(out StringObj s))
                                throw new InterpreterException($"Instruction object was of type '{instruction.Object.TypeName}', expected 'string'.");

                            var callFunction = _class.FindFunction(s.Text);
                            if (callFunction == null)
                                throw new InterpreterException($"Class '{_class.Name}' does not define function '{s.Text}'.");

                            _frames.Push(new CallFrame(callFunction));
                            callFunction.Scope.Reset();
                            callFunction.Scope.SetParent(_class.Scope);
                            _scopes.Push(callFunction.Scope);
                        }
                        break;
                    case OperationCode.RETURN:
                        {
                            if (frame.Function?.Name == "main") // if function main() returns an object, return the value to the C# object calling the script
                                return _stack.Pop();
                            _frames.Pop();
                            _scopes.Pop();
                        }
                        break;
                    case OperationCode.ECALL:
                        {
                            if (!_stack.Peek().TryGetNumber(out Number n))
                                throw new InterpreterException($"Object on top of stack was of type '{_stack.Peek().TypeName}', expected 'number'.");

                            double numArgs = n.Value;
                            _stack.Pop();

                            var args = new List<ScriptObject>();
                            for(int i = 0; i < numArgs; i++)
                            {
                                args.Add(_stack.Peek()); // What's the reason for not just using _stack.Pop() in the call to args.Add()? Am I too tired to reason well or is this unnecessary?
                                _stack.Pop();
                            }

                            if (!instruction.Object.TryGetString(out StringObj s))
                                throw new InterpreterException($"Instruction object was of type '{instruction.Object.TypeName}', expected 'string'.");

                            var exportedFunction = _environment.FindExportedFunction(s.Text);

                            if (exportedFunction == null)
                                throw new InterpreterException($"exported function '{s.Text}' does not exist.");

                            if (args.Count < exportedFunction.ArgumentCount.Min || args.Count > exportedFunction.ArgumentCount.Max)
                                throw new InterpreterException($"exported function '{s.Text}' requires {exportedFunction.ArgumentCount} argument(s).");

                            var returnValue = exportedFunction.Call(_environment, args);
                            if (returnValue != null)
                                _stack.Push(returnValue);
                            else
                                _stack.Push(ScriptEnvironment.NullObject);
                        }
                        break;
                    case OperationCode.MAKE_TABLE:
                        {
                            // get amount of string keys
                            if (!_stack.Peek().TryGetNumber(out Number stringValueCount))
                                throw new InterpreterException($"object was '{_stack.Peek().TypeName}', expected 'number'.");
                            _stack.Pop();

                            var table = new Table();

                            // add string keys + values to table
                            var stringNum = stringValueCount.Value;
                            for(int i = 0; i < stringNum; i++)
                            {
                                var stringKey = _stack.Peek() as StringObj;
                                _stack.Pop();
                                var val = _stack.Peek();
                                _stack.Pop();
                                table.StringValues.Add(stringKey.Text, val);
                            }

                            if(!_stack.Peek().TryGetNumber(out Number intValueCount))
                                throw new InterpreterException($"object was '{_stack.Peek().TypeName}', expected 'number'.");
                            _stack.Pop();

                            // add int keys + values to table
                            var intNum = intValueCount.Value;
                            for(int i = 0; i < intNum; i++)
                            {
                                var intKey = _stack.Peek() as Number;
                                _stack.Pop();
                                var val = _stack.Peek();
                                _stack.Pop();
                                table.IntegerValues.Add((int)intKey.Value, val);
                            }

                            // table is done
                            _stack.Push(table); // push completed table
                        }
                        break;
                    case OperationCode.COUNT:
                        {
                            if (!_stack.Peek().TryGetTable(out Table t))
                                throw new InterpreterException($"expected 'table' object on top of stack.");
                            _stack.Pop();
                            _stack.Push(new Number(t.Count)); // push table element count
                        }
                        break;
                    case OperationCode.SET:
                        {
                            if (!instruction.Object.TryGetString(out StringObj s))
                                throw new InterpreterException($"Instruction object was of type '{instruction.Object.TypeName}', expected 'string'.");

                            _scopes.Peek().Set(s.Text, _stack.Peek());
                        }
                        break;
                    case OperationCode.DEF:
                        {
                            if (!instruction.Object.TryGetString(out StringObj s))
                                throw new InterpreterException($"Instruction object was of type '{instruction.Object.TypeName}', expected 'string'.");

                            _scopes.Peek().Define(s.Text, _stack.Peek());
                        }
                        break;
                    case OperationCode.REF:
                        {
                            if (!instruction.Object.TryGetString(out StringObj s))
                                throw new InterpreterException($"Instruction object was of type '{instruction.Object.TypeName}', expected 'string'.");
                            var arg = _scopes.Peek().Find(s.Text);
                            if (instruction.Argument != Compiler.REFERENCE_TABLE_INDEX)
                            {
                                _stack.Push(arg);
                                break;
                            }

                            // REF table
                            if (!arg.TryGetTable(out Table t))
                                throw new InterpreterException($"Referenced object was of type '{instruction.Object.TypeName}', expected 'table'.");
                            if(!_stack.Peek().TryGetNumber(out Number n))
                                throw new InterpreterException($"Object on top of stack was of type '{_stack.Peek().TypeName}', expected 'number'.");
                            var indexCount = n.Value;
                            _stack.Pop();

                            var indexes = new List<ScriptObject>(); // go through the indexes
                            for(int i = 0; i < indexCount; i++)
                            {
                                indexes.Add(_stack.Peek());
                                _stack.Pop();
                            }

                            var table = new Table(); // create a new table
                            for(int i = 0; i < indexes.Count; i++)
                            {
                                // fill the table with the objects from the specified indexes
                                if (indexes[i].TryGetNumber(out Number index))
                                {
                                    var valueAtIndex = t.Find((int)index.Value);
                                    if (valueAtIndex == null)
                                        throw new InterpreterException($"No element found at index '{index.Value}' in table '{s.Text}'.");
                                    table.IntegerValues.Add(table.IntegerValues.Count, valueAtIndex);
                                }
                                else if (indexes[i].TryGetString(out StringObj strIndex))
                                {
                                    var valueAtIndex = t.Find(strIndex.Text);
                                    if(valueAtIndex == null)
                                        throw new InterpreterException($"No element found at index '{strIndex.Text}' in table '{s.Text}'.");
                                    table.IntegerValues.Add(table.IntegerValues.Count, valueAtIndex);
                                }
                            }

                            if(table.Count != 1) // if count is not 1, return the new table
                                _stack.Push(table);
                            else // if count is 1, just push the object at that index
                            {
                                if (table.IntegerValues.Count == 1)
                                    _stack.Push(table.IntegerValues.First().Value);
                                else if (table.StringValues.Count == 1)
                                    _stack.Push(table.StringValues.First().Value);
                            }
                        }
                        break;
                    case OperationCode.BRANCH:
                        {
                            frame.Address = (int)instruction.Argument; 
                            // I have a feeling this is going to crash because of the uint to int cast, if a script would ever have an address over the maximum value of int
                        }
                        break;
                    case OperationCode.BRANCH_IF_TRUE:
                        {
                            if(_stack.Peek().Type == ObjectType.TRUE)
                            {
                                frame.Address = (int)instruction.Argument; // same thing here..
                                // maybe I should make an implementation of List<T> using only uint instead of int?
                            }
                            _stack.Pop();
                        }
                        break;
                    case OperationCode.BRANCH_IF_FALSE:
                        {
                            if(_stack.Peek().Type == ObjectType.FALSE)
                            {
                                frame.Address = (int)instruction.Argument;
                            }
                            _stack.Pop();
                        }
                        break;
                    case OperationCode.ADD:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            switch(left.Type)
                            {
                                case ObjectType.NUMBER:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.NUMBER:
                                                {
                                                    //if (!left.TryGetNumber(out Number lNum) || !right.TryGetNumber(out Number rNum))
                                                    //    throw new InterpreterException($"What just happened?");
                                                    _stack.Push(new Number(((Number)left).Value + ((Number)right).Value));
                                                }
                                                break;
                                            case ObjectType.STRING:
                                                {
                                                    _stack.Push(new StringObj(((Number)left).Value + ((StringObj)right).Text));
                                                }
                                                break;
                                            default:
                                                {
                                                    throw new InterpreterException($"Type mismatch on '+' operator.");
                                                }
                                        }
                                    }
                                    break;
                                case ObjectType.STRING:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.STRING:
                                                {
                                                    //if (!left.TryGetString(out StringObj lStr) || !right.TryGetString(out StringObj rStr))
                                                    //    throw new InterpreterException($"What just happened?");
                                                    _stack.Push(new StringObj(((StringObj)left).Text + ((StringObj)right).Text));
                                                }
                                                break;
                                            case ObjectType.NUMBER:
                                                {
                                                    _stack.Push(new StringObj(((StringObj)left).Text + ((Number)right).Value));
                                                }
                                                break;
                                            default:
                                                {
                                                    throw new InterpreterException($"Type mismatch on '+' operator.");
                                                }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        throw new InterpreterException($"Invalid arguments to '+' operator.");
                                    }
                            }
                        }
                        break;
                    case OperationCode.SUB:
                        {
                            var right = _stack.Peek(); // TODO: this seems unnecessary.. maybe the reason is to be more.. verbose?
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            switch(left.Type)
                            {
                                case ObjectType.NUMBER:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.NUMBER:
                                                {
                                                    _stack.Push(new Number(((Number)left).Value - ((Number)right).Value));
                                                }
                                                break;
                                            default:
                                                {
                                                    throw new InterpreterException($"Type mismatch on '-' operator.");
                                                }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        throw new InterpreterException($"Invalid arguments to '-' operator.");
                                    }
                            }
                        }
                        break;
                    case OperationCode.MUL:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            switch(left.Type)
                            {
                                case ObjectType.NUMBER:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.NUMBER:
                                                {
                                                    _stack.Push(new Number(((Number)left).Value * ((Number)right).Value));
                                                }
                                                break;
                                            default:
                                                {
                                                    throw new InterpreterException($"Type mismatch on '*' operator.");
                                                }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        throw new InterpreterException($"Invalid arguments to '*' operator.");
                                    }
                            }
                        }
                        break;
                    case OperationCode.DIV:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            switch (left.Type)
                            {
                                case ObjectType.NUMBER:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.NUMBER:
                                                {
                                                    if (((Number)right).Value == 0)
                                                        _stack.Push(ScriptEnvironment.NullObject);
                                                    else
                                                        _stack.Push(new Number(((Number)left).Value / ((Number)right).Value));
                                                }
                                                break;
                                            default:
                                                {
                                                    throw new InterpreterException($"Type mismatch on '/' operator.");
                                                }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        throw new InterpreterException($"Invalid arguments to '/' operator.");
                                    }
                            }
                        }
                        break;
                    case OperationCode.NEG:
                        {
                            var value = _stack.Peek();
                            _stack.Pop();

                            switch(value.Type)
                            {
                                case ObjectType.NUMBER:
                                    {
                                        _stack.Push(new Number(-(((Number)value).Value))); // TODO: Test this - you're tired, it may be all wrong
                                    }
                                    break;
                                default:
                                    {
                                        throw new InterpreterException($"Invalid arguments to '-' operator.");
                                    }
                            }
                        }
                        break;
                    case OperationCode.INC:
                        {
                            var identifier = _stack.Peek();
                            switch(identifier.Type)
                            {
                                case ObjectType.NUMBER:
                                    {
                                        var num = (Number)identifier;
                                        num.Set(num.Value + 1);
                                    }
                                    break;
                                case ObjectType.NULL:
                                    throw new InterpreterException("Attempt to increment an uninitialized variable.");
                                default:
                                    throw new InterpreterException("Attempt to increment invalid value.");
                            }
                        }
                        break;
                    case OperationCode.DEC:
                        {
                            var identifier = _stack.Peek();
                            switch (identifier.Type)
                            {
                                case ObjectType.NUMBER:
                                    {
                                        var num = (Number)identifier;
                                        num.Set(num.Value - 1);
                                    }
                                    break;
                                case ObjectType.NULL:
                                    throw new InterpreterException("Attempt to decrement an uninitialized variable.");
                                default:
                                    throw new InterpreterException("Attempt to decrement invalid value.");
                            }
                        }
                        break;
                    case OperationCode.NOT:
                        {
                            var value = _stack.Peek();
                            _stack.Pop();

                            switch(value.Type)
                            {
                                case ObjectType.TRUE:
                                    {
                                        _stack.Push(ScriptEnvironment.FalseObject);
                                    }
                                    break;
                                case ObjectType.FALSE:
                                    {
                                        _stack.Push(ScriptEnvironment.TrueObject);
                                    }
                                    break;
                                default:
                                    throw new InterpreterException("Invalid arguments to '!' operator.");
                            }
                        }
                        break;
                    case OperationCode.OR:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            switch(left.Type)
                            {
                                case ObjectType.TRUE:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.TRUE:
                                                {
                                                    _stack.Push(ScriptEnvironment.TrueObject);
                                                }
                                                break;
                                            case ObjectType.FALSE:
                                                {
                                                    _stack.Push(ScriptEnvironment.TrueObject);
                                                }
                                                break;
                                            default:
                                                throw new InterpreterException("Type mismatch on '||' operator.");
                                        }
                                    }
                                    break;
                                case ObjectType.FALSE:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.TRUE:
                                                {
                                                    _stack.Push(ScriptEnvironment.TrueObject);
                                                }
                                                break;
                                            case ObjectType.FALSE:
                                                {
                                                    _stack.Push(ScriptEnvironment.FalseObject);
                                                }
                                                break;
                                            default:
                                                throw new InterpreterException("Type mismatch on '||' operator.");
                                        }
                                    }
                                    break;
                                default:
                                    throw new InterpreterException("Invalid arguments to '||' operator.");
                            }
                        }
                        break;
                    case OperationCode.AND:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            switch(left.Type)
                            {
                                case ObjectType.TRUE:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.TRUE:
                                                {
                                                    _stack.Push(ScriptEnvironment.TrueObject);
                                                }
                                                break;
                                            case ObjectType.FALSE:
                                                {
                                                    _stack.Push(ScriptEnvironment.FalseObject);
                                                }
                                                break;
                                            default:
                                                throw new InterpreterException("Type mismatch on '&&' operator.");
                                        }
                                    }
                                    break;
                                case ObjectType.FALSE:
                                    {
                                        switch(right.Type)
                                        {
                                            case ObjectType.TRUE:
                                                {
                                                    _stack.Push(ScriptEnvironment.FalseObject);
                                                }
                                                break;
                                            case ObjectType.FALSE:
                                                {
                                                    _stack.Push(ScriptEnvironment.FalseObject);
                                                }
                                                break;
                                            default:
                                                throw new InterpreterException("Type mismatch on '&&' operator.");
                                        }
                                    }
                                    break;
                                default:
                                    throw new InterpreterException("Invalid arguments to '&&' operator.");
                            }
                        }
                        break;
                    case OperationCode.EQEQ:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            if (left == right) // TODO: Not sure if this will actually hit any of the custom '==' operators defined in the classes?
                                _stack.Push(ScriptEnvironment.TrueObject);
                            else
                                _stack.Push(ScriptEnvironment.FalseObject);
                        }
                        break;
                    case OperationCode.NEQ:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            if (left != right) // TODO: Same doubt as in case OperationCode.EQEQ
                                _stack.Push(ScriptEnvironment.TrueObject);
                            else
                                _stack.Push(ScriptEnvironment.FalseObject);
                        }
                        break;
                    case OperationCode.LT:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            if (left < right)
                                _stack.Push(ScriptEnvironment.TrueObject);
                            else
                                _stack.Push(ScriptEnvironment.FalseObject);
                        }
                        break;
                    case OperationCode.GT:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            if (left > right)
                                _stack.Push(ScriptEnvironment.TrueObject);
                            else
                                _stack.Push(ScriptEnvironment.FalseObject);
                        }
                        break;
                    case OperationCode.LTE:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            if (left <= right)
                                _stack.Push(ScriptEnvironment.TrueObject);
                            else
                                _stack.Push(ScriptEnvironment.FalseObject);
                        }
                        break;
                    case OperationCode.GTE:
                        {
                            var right = _stack.Peek();
                            _stack.Pop();

                            var left = _stack.Peek();
                            _stack.Pop();

                            if (left >= right)
                                _stack.Push(ScriptEnvironment.TrueObject);
                            else
                                _stack.Push(ScriptEnvironment.FalseObject);
                        }
                        break;
                }
            }
            return ScriptEnvironment.NullObject; // return null if nothing else
        }

    }
}
