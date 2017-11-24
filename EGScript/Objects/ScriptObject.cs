using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Objects
{
    public enum ObjectType
    {
        NUMBER,
        STRING,
        TABLE,
        INSTANCE,
        TRUE,
        FALSE,
        NULL
    }

    public abstract class ScriptObject
    {
        public virtual ObjectType Type { get; }
        public string TypeName
        {
            get
            {
                switch (Type)
                {
                    case ObjectType.NUMBER: return "number";
                    case ObjectType.STRING: return "string";
                    case ObjectType.TABLE: return "table";
                    case ObjectType.INSTANCE: return "instance";
                    case ObjectType.NULL: return "null";
                    case ObjectType.TRUE:
                    case ObjectType.FALSE:
                        return "boolean";
                    default:
                        return "undefined";
                }
            }
        }

        public static bool operator ==(ScriptObject left, ScriptObject right)
        {
            if ((object)left == null && (object)right == null) return true;
            else if (left == (object)null) return false;
            else if (right == (object)null) return false;
            // TODO: Refactor?
            switch(left.Type)
            {
                case ObjectType.NUMBER:
                    return ((Number)left) == right;
                case ObjectType.STRING:
                    return ((StringObj)left) == right;
                case ObjectType.TABLE:
                    return ((Table)left) == right;
                case ObjectType.TRUE:
                    return ((True)left) == right;
                case ObjectType.FALSE:
                    return ((False)left) == right;
                case ObjectType.NULL:
                    return ((Null)left) == right;
                case ObjectType.INSTANCE:
                    return ((Instance)left) == right;
            }
            throw new OperatorException("==", left.TypeName, left.Type, right);
        }

        public static bool operator !=(ScriptObject left, ScriptObject right)
        {
            return !(left == right);
        }

        public static bool operator <(ScriptObject left, ScriptObject right)
        {
            // TODO: Refactor?
            switch (left.Type)
            {
                case ObjectType.NUMBER:
                    return ((Number)left) < right;
                case ObjectType.STRING:
                    return ((StringObj)left) < right;
            }
            throw new OperatorException("<", left.TypeName, left.Type, right);
        }

        public static bool operator >(ScriptObject left, ScriptObject right)
        {
            // TODO: Refactor?
            switch (left.Type)
            {
                case ObjectType.NUMBER:
                    return ((Number)left) > right;
                case ObjectType.STRING:
                    return ((StringObj)left) > right;
            }
            throw new OperatorException(">", left.TypeName, left.Type, right);
        }

        public static bool operator <=(ScriptObject left, ScriptObject right)
        {
            // TODO: Refactor?
            switch (left.Type)
            {
                case ObjectType.NUMBER:
                    return ((Number)left) <= right;
                case ObjectType.STRING:
                    return ((StringObj)left) <= right;
            }
            throw new OperatorException("<=", left.TypeName, left.Type, right);
        }

        public static bool operator >=(ScriptObject left, ScriptObject right)
        {
            return !(left <= right);
        }

        public virtual bool TryGetNumber(out Number n) { n = null; return false; }
        public virtual bool TryGetNull(out Null n) { n = null; return false; }
        public virtual bool TryGetString(out StringObj s) { s = null; return false; }
        public virtual bool TryGetTrue(out True t) { t = null; return false; }
        public virtual bool TryGetFalse(out False f) { f = null; return false; }
        public virtual bool TryGetInstance(out Instance i) { i = null; return false; }
        public virtual bool TryGetTable(out Table t) { t = null; return false; }
    }
}
