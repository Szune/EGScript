using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Objects
{
    public class Number : ScriptObject
    {
        public override ObjectType Type => ObjectType.NUMBER;
        public double Value { get; private set; }
        public Number(double number)
        {
            Value = number;
        }

        public void Set(double number)
        {
            Value = number;
        }

        public override bool TryGetNumber(out Number n)
        {
            n = this;
            return true;
        }

        public static bool operator ==(Number number, ScriptObject other)
        {
            if(other.TryGetNumber(out Number n))
                return number.Value == n.Value;

            else if (other.TryGetString(out StringObj s))
                if (IsStringDouble(s.Text, out double val))
                    return number.Value == val;
                else
                    return false;
            else if (other.TryGetFalse(out False f) || other.TryGetNull(out Null nul))
                return number.Value == 0;
            else if (other.TryGetTrue(out True t))
                return number.Value != 0;

            throw new OperatorException("==", number.ToString(), ObjectType.NUMBER, other);
        }

        public static bool operator !=(Number number, ScriptObject other)
        {
            return !(number == other);
        }

        public static bool operator <(Number number, ScriptObject other)
        {
            if (other.TryGetNumber(out Number n))
                return number.Value < n.Value;
            else if (other.TryGetString(out StringObj s))
                if (IsStringDouble(s.Text, out double val))
                    return number.Value < val;
                else
                    return false;
            throw new OperatorException("<", number.ToString(), ObjectType.NUMBER, other);
        }

        public static bool operator >(Number number, ScriptObject other)
        {
            if (other.TryGetNumber(out Number n))
                return number.Value > n.Value;
            else if (other.TryGetString(out StringObj s))
                if (IsStringDouble(s.Text, out double val))
                    return number.Value > val;
                else
                    return false;
            throw new OperatorException(">", number.ToString(), ObjectType.NUMBER, other);
        }

        public static bool operator <=(Number number, ScriptObject other)
        {
            if (other.TryGetNumber(out Number n))
                return number.Value <= n.Value;

            else if (other.TryGetString(out StringObj s))
                if (IsStringDouble(s.Text, out double val))
                    return number.Value <= val;
                else
                    return false;
            throw new OperatorException("<=", number.ToString(), ObjectType.NUMBER, other);
        }

        public static bool operator >=(Number number, ScriptObject other)
        {
            return !(number <= other);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private static bool IsStringDouble(string s, out double val)
        {
            val = default(double);
            if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out double value))
            {
                val = value;
                return true;
            }
            return false;
        }
    }
}
