using System.Collections.Generic;
using EGScript.Objects;
using EGScript.Scripter;

namespace EGScript.AbstractSyntaxTree
{
    public class ASTTable : ASTExpressionBase
    {
        public override ExpressionType Type => ExpressionType.TABLE;
        public Dictionary<string, ASTTableElement> StringValues { get; }
        public Dictionary<int, ASTTableElement> IntegerValues { get; }
        private Dictionary<int, ASTTableElement> AutoIntegerValues { get; } // only used when normalizing variables, therefore should not be accessed outside this class

        public ASTTable()
        {
            StringValues = new Dictionary<string, ASTTableElement>();
            IntegerValues = new Dictionary<int, ASTTableElement>();
            AutoIntegerValues = new Dictionary<int, ASTTableElement>();
        }

        public override void Accept(IVisitor visitor, Function function)
        {
            visitor.Visit(this, function);
        }

        public bool InsertValue(ASTExpressionBase key, ASTExpressionBase value)
        {
            if(key == null)
            {
                AutoIntegerValues.Add(AutoIntegerValues.Count, new ASTTableElement(AutoIntegerValues.Count, value));
                return true;
            }

            if (key.Type == ExpressionType.NUMBER)
            {
                var numberKey = key as ASTNumber;
                if (IntegerValues.ContainsKey((int)numberKey.Value)) // TODO: Do something to stop this from possibly crashing.. someday. (double to int cast)
                    return false;
                IntegerValues.Add((int)numberKey.Value, new ASTTableElement((int)numberKey.Value, value));
            }
            else if (key.Type == ExpressionType.STRING)
            {
                var stringKey = key as ASTString;
                if (StringValues.ContainsKey(stringKey.Text))
                    return false;
                StringValues.Add(stringKey.Text, new ASTTableElement(stringKey.Text,value));
            }
            else if (key.Type == ExpressionType.IDENTIFIER)
            {
                var identifierKey = key as ASTIdentifier;
                if (StringValues.ContainsKey(identifierKey.Name))
                    return false;
                StringValues.Add(identifierKey.Name, new ASTTableElement(identifierKey.Name, value));
            }
            else
                return false;
            return true;
        }

        /// <summary>
        /// Inserts all of the values that were specified without keys at the first open integer indexes.
        /// </summary>
        public void NormalizeValues()
        {
            for(int autoInt = 0; autoInt < AutoIntegerValues.Count; autoInt++)
            {
                for(int i = 0;; i++)
                {
                    if(!IntegerValues.ContainsKey(i))
                    {
                        IntegerValues.Add(i, AutoIntegerValues[autoInt]);
                        break;
                    }
                }
            }

            AutoIntegerValues.Clear();

            /* Rest of the code in C++ is simply outputting the table to the console (I think), probably for debugging purposes */
        }
    }
}
