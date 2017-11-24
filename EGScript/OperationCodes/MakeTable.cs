using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGScript.Scripter;
using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class MakeTable : OperationCodeBase
    {
        public override void Execute(InterpreterState state)
        {
            // get amount of string keys
            if (!state.Stack.Peek().TryGetNumber(out Number stringValueCount))
                throw new InterpreterException($"object was '{state.Stack.Peek().TypeName}', expected 'number'.");
            state.Stack.Pop();

            var table = new Table();

            // add string keys + values to table
            var stringNum = stringValueCount.Value;
            for (int i = 0; i < stringNum; i++)
            {
                var stringKey = state.Stack.Peek() as StringObj;
                state.Stack.Pop();
                var val = state.Stack.Peek();
                state.Stack.Pop();
                table.StringValues.Add(stringKey.Text, val);
            }

            if (!state.Stack.Peek().TryGetNumber(out Number intValueCount))
                throw new InterpreterException($"object was '{state.Stack.Peek().TypeName}', expected 'number'.");
            state.Stack.Pop();

            // add int keys + values to table
            var intNum = intValueCount.Value;
            for (int i = 0; i < intNum; i++)
            {
                var intKey = state.Stack.Peek() as Number;
                state.Stack.Pop();
                var val = state.Stack.Peek();
                state.Stack.Pop();
                table.IntegerValues.Add((int)intKey.Value, val);
            }

            // table is done
            state.Stack.Push(table); // push completed table
        }
    }
}
