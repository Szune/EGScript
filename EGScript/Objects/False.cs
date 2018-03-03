namespace EGScript.Objects
{
    public class False : ScriptObject
    {
        public override ObjectType Type => ObjectType.FALSE;

        public override string ToString()
        {
            return "false";
        }

        public override bool TryGetFalse(out False f)
        {
            f = this;
            return true;
        }

        public static bool operator ==(False fals, ScriptObject other)
        {
            if (other.TryGetTrue(out True t))
                return false;
            if (other.TryGetFalse(out False f) || other.TryGetNull(out Null n))
                return true;
            return (other == fals); // TODO: Possible error caused by bad translation from C++ here, test extensively!
        }

        public static bool operator !=(False fals, ScriptObject other)
        {
            return !(fals == other);
        }
    }
}
