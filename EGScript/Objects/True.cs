using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Objects
{
    public class True : ScriptObject
    {
        public override ObjectType Type => ObjectType.TRUE;

        public True()
        {

        }

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
            else if (other.TryGetFalse(out False f) || other.TryGetNull(out Null n))
                return false;
            else
                return (other == (tru as ScriptObject)); // TODO: Possible error caused by bad translation from C++ here, test extensively!
        }

        public static bool operator !=(True tru, ScriptObject other)
        {
            return !(tru == other);
        }
    }
}
