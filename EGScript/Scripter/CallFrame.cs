using EGScript.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class CallFrame
    {
        public int Address; // the only reason this is an int is because lists use int and CodeBlock uses a list
        public Instance Instance;
        public Function Function;

        public CallFrame(Function func)
        {
            Address = 0;
            Function = func;
        }

        public CallFrame(Function func, Instance instance) : this(func)
        {
            Instance = instance;
        }
    }
}
