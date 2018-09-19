namespace EGScript.Objects
{
    public class StringObj : ScriptObject
    {
        public override ObjectType Type => ObjectType.STRING;
        public string Text { get; private set; }

        public StringObj(string text)
        {
            Text = Unescape(text);
        }

        public void Set(string text)
        {
            Text = Unescape(text);
        }

        public override bool TryGetString(out StringObj s)
        {
            s = this;
            return true;
        }

        public static bool operator ==(StringObj str, ScriptObject other)
        {
            if((object)str == null && (object)other == null)
                return true;
            if ((object) str == null || (object) other == null)
                return false;
            if (other.TryGetNumber(out Number n))
                return n == str;
            if (other.TryGetString(out StringObj s))
                return (str.Text.CompareTo(s.Text) == 0);
            if (other.TryGetFalse(out False f) || other.TryGetNull(out Null nul))
                return false;
            if (other.TryGetTrue(out True t))
                return true;

            throw new OperatorException("==", str.ToString(), ObjectType.STRING, other);
        }

        public static bool operator !=(StringObj str, ScriptObject other)
        {
            return !(str == other);
        }

        public static bool operator <(StringObj str, ScriptObject other)
        {
            if (other.TryGetNumber(out Number n))
                return n > str;
            if (other.TryGetString(out StringObj s))
                return str.Text.CompareTo(s.Text) < 0; // TODO: Possibly remove / adjust this

            throw new OperatorException("<", str.ToString(), ObjectType.STRING, other);
        }

        public static bool operator >(StringObj str, ScriptObject other)
        {
            if (other.TryGetNumber(out Number n))
                return n < str;
            if (other.TryGetString(out StringObj s))
                return str.Text.CompareTo(s.Text) > 0; // TODO: Possibly remove / adjust this

            throw new OperatorException(">", str.ToString(), ObjectType.STRING, other);
        }

        public static bool operator <=(StringObj str, ScriptObject other)
        {
            if (other.TryGetNumber(out Number n))
                return (n >= str);
            if (other.TryGetString(out StringObj s))
                return str.Text.CompareTo(s.Text) <= 0; // TODO: Possibly remove / adjust this

            throw new OperatorException("<=", str.ToString(), ObjectType.STRING, other);
        }

        public static bool operator >=(StringObj str, ScriptObject other)
        {
            return !(str <= other);
        }

        public override string ToString()
        {
            return Text;
        }

        private string Unescape(string s)
        {
            string res = "";
            int i = 0;
            while(i < s.Length)
            {
                char c = s[i++]; // fetches char _before_ incrementing 'i'
                if(c == '\\' && i < s.Length - 1) // if current char is '\' and there is at least one char ahead of the current one ->
                {
                    switch (s[i++]) // act depending on what the next char is
                    {
                        case '\\': c = '\\'; break;
                        case 'a': c = '\a'; break;
                        case 'b': c = '\b'; break;
                        case 'f': c = '\f'; break;
                        case 'n': c = '\n'; break;
                        case 'r': c = '\r'; break;
                        case 't': c = '\t'; break;
                        case 'v': c = '\v'; break;
                        default:
                            continue;
                    }
                }
                res += c;
            }

            return res;
        }
    }
}
