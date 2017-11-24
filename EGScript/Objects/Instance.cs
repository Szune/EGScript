using EGScript.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Objects
{
    public class Instance : ScriptObject
    {
        public override ObjectType Type => ObjectType.INSTANCE;

        public Class Class { get; }
        public Scope Scope { get; set; }

        public Instance(Class _class)
        {
            Class = _class;
            Scope = new Scope(_class.Scope);
        }

        public override bool TryGetInstance(out Instance i)
        {
            i = this;
            return true;
        }

        public static bool operator ==(Instance instance, ScriptObject other)
        {
            if (other.TryGetInstance(out Instance i))
                return instance.Scope == i.Scope;

            throw new OperatorException("==", instance.TypeName, ObjectType.INSTANCE, other);
        }

        public static bool operator !=(Instance instance, ScriptObject other)
        {
            return !(instance == other);
        }
    }
}
