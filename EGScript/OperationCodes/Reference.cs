using System.Collections.Generic;
using System.Linq;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.OperationCodes
{
    // TODO: Should probably refactor the table stuff
    /// <summary>
    /// References a variable from the stack.
    /// </summary>
    public class Reference : OperationCodeBase
    {
        public Reference(ScriptObject variableName)
        {
            VariableName = variableName;
        }

        public Reference(ScriptObject variableName, uint argument) : this(variableName)
        {
            Argument = argument;
        }

        public ScriptObject VariableName { get; }
        public uint Argument { get; }

        public override void Execute(InterpreterState state)
        {
            if (!VariableName.TryGetString(out StringObj s))
                throw new InterpreterException($"Instruction object was of type '{VariableName.TypeName}', expected 'string'.");

            // check for global vars
            var arg = state.Environment.Globals.Scope.Find(s.Text);
            if (arg != null && Argument != Compiler.ReferenceTableIndex)
            {
                state.Stack.Push(arg);
                return;
            }

            // check for local vars
            arg = state.Scopes.Peek().Find(s.Text);
            if (Argument != Compiler.ReferenceTableIndex)
            { 
                state.Stack.Push(arg);
                return;
            }

            // REF table
            if (!arg.TryGetTable(out Table t))
                throw new InterpreterException($"Referenced object was of type '{arg.TypeName}', expected 'table'.");

            if (!state.Stack.Peek().TryGetNumber(out Number n))
                throw new InterpreterException($"Object on top of stack was of type '{state.Stack.Peek().TypeName}', expected 'number'.");
            var indexCount = n.Value;
            state.Stack.Pop();

            var indexes = new List<ScriptObject>(); // go through the indexes
            for (int i = 0; i < indexCount; i++)
            {
                indexes.Add(state.Stack.Peek());
                state.Stack.Pop();
            }

            var table = new Table(); // create a new table
            for (int i = 0; i < indexes.Count; i++)
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
                    if (valueAtIndex == null)
                        throw new InterpreterException($"No element found at index '{strIndex.Text}' in table '{s.Text}'.");
                    table.IntegerValues.Add(table.IntegerValues.Count, valueAtIndex);
                }
            }

            if (table.Count != 1) // if count is not 1, return the new table
                state.Stack.Push(table);
            else // if count is 1, just push the object at that index
            {
                if (table.IntegerValues.Count == 1)
                    state.Stack.Push(table.IntegerValues.First().Value);
                else if (table.StringValues.Count == 1)
                    state.Stack.Push(table.StringValues.First().Value);
            }
        }
    }
}
