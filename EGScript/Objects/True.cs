namespace EGScript.Objects
{
    public class True : ScriptObject
    {
        public override ObjectType Type => ObjectType.TRUE;

        public override string ToString()
        {
            return "true";
        }

        public override bool TryGetTrue(out True t)
        {
            t = this;
            return true;
        }

        public static bool operator ==(True tru, ScriptObject other)
        {
            if (other.TryGetTrue(out True t))
                return true;
            if (other.TryGetFalse(out False f) || other.TryGetNull(out Null n))
                return false;
            return (other == tru); // TODO: Possible error caused by bad translation from C++ here, test extensively!
        }

        public static bool operator !=(True tru, ScriptObject other)
        {
            return !(tru == other);
        }
    }
}
