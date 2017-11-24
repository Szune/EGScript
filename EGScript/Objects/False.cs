using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Objects
{
    public class False : ScriptObject
    {
        public override ObjectType Type => ObjectType.FALSE;

        public False()
        {

        }

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
            else if (other.TryGetFalse(out False f) || other.TryGetNull(out Null n))
                return true;
            else
                return (other == (fals as ScriptObject)); // TODO: Possible error caused by bad translation from C++ here, test extensively!
        }

        public static bool operator !=(False fals, ScriptObject other)
        {
            return !(fals == other);
        }
    }
}
