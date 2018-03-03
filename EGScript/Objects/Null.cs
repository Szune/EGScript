namespace EGScript.Objects
{
    public class Null : ScriptObject
    {
        public override ObjectType Type => ObjectType.NULL;
        public override bool TryGetNull(out Null n)
        {
            n = this;
            return true;
        }

        public override string ToString()
        {
            return "null";
        }

        public static bool operator ==(Null nullObj, ScriptObject other)
        {
            if (other.TryGetTrue(out True t))
                return false;
            if (other.TryGetFalse(out False f) || other.TryGetNull(out Null n))
                return true;
            return (other == nullObj); // TODO: Possible error caused by bad translation from C++ here, test extensively!
        }

        public static bool operator !=(Null nullObj, ScriptObject other)
        {
            return !(nullObj == other);
        }
    }
}
