using System.Collections.Generic;

namespace EGScript.Objects
{
    public class Table : ScriptObject
    {
        public override ObjectType Type => ObjectType.TABLE;
        public Dictionary<int, ScriptObject> IntegerValues { get; }
        public Dictionary<string, ScriptObject> StringValues { get; }
        public int Count => IntegerValues.Count + StringValues.Count;

        public Table()
        {
            IntegerValues = new Dictionary<int, ScriptObject>();
            StringValues = new Dictionary<string, ScriptObject>();
        }

        public Table(Dictionary<int, ScriptObject> integerValues, Dictionary<string, ScriptObject> stringValues)
        {
            IntegerValues = integerValues;
            StringValues = stringValues;
        }

        public override bool TryGetTable(out Table t)
        {
            t = this;
            return true;
        }

        public static bool operator ==(Table tbl, ScriptObject other)
        {
            if(other.TryGetTable(out Table t)) // TODO: use a better form of comparison, currently only compares element count
            {
                if (tbl.Count != t.Count) return false;
                return true;
            }

            throw new OperatorException("==", tbl.TypeName, ObjectType.TABLE, other);
        }

        public static bool operator !=(Table tbl, ScriptObject other)
        {
            return !(tbl == other);
        }

        public ScriptObject Find(string stringValue)
        {
            if (StringValues.ContainsKey(stringValue))
                return StringValues[stringValue];
            return null;
        }

        public ScriptObject Find(int integerValue)
        {
            if (IntegerValues.ContainsKey(integerValue))
                return IntegerValues[integerValue];
            return null;
        }
    }
}
