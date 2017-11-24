using EGScript.Objects;
using System.Collections.Generic;

namespace EGScript.Scripter
{
    public class Scope
    {
        private SortedDictionary<string, ScriptObject> _variables;
        private Scope _parent;
        public Scope Parent => _parent;

        public Scope()
        {
            _variables = new SortedDictionary<string, ScriptObject>();
        }

        public static Scope Copy(Scope copy)
        {
            Scope newScope = new Scope
            {
                _variables = copy._variables
            };
            if (copy._parent != null)
                newScope._parent = new Scope(copy._parent);

            return newScope;
        }

        public Scope(Scope parent) : this()
        {
            _parent = parent;
        }

        public void Define(string name)
        {
            _variables.Add(name, ObjectFactory.Null);
        }

        public void Define(string name, ScriptObject obj)
        {
            _variables.Add(name, obj);
        }

        public void Set(string name, ScriptObject obj)
        {
            if (!_variables.ContainsKey(name))
            { 
                if (_parent == null)
                    throw new ScopeException($"Variable {name} has not been defined.");
                _parent.Set(name, obj);
            }
            _variables[name] = obj;
        }

        public void SetParent(Scope parent)
        {
            _parent = parent;
        }

        public void Clear()
        {
            _variables.Clear();
        }

        public void Reset()
        {
            var tmpList = new List<string>(_variables.Keys);
            foreach(var vari in tmpList)
                _variables[vari] = ObjectFactory.Null;
        }

        public ScriptObject Find(string name)
        {
            if(_variables.TryGetValue(name, out ScriptObject vari))
            {
                return vari;
            }
            else
            {
                if(_parent != null)
                    return _parent.Find(name);
                return null;
            }
        }


    }
}
