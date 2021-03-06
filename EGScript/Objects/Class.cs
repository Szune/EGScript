﻿using System.Collections.Generic;
using System.Linq;
using EGScript.Scripter;

namespace EGScript.Objects
{
    public class Class
    {
        public string Name { get; }
        public Class Base { get; } 
        public Scope Scope { get; }
        private List<Function> Functions { get; }

        public Class(string name)
        {
            Name = name;
            Scope = new Scope();
            Functions = new List<Function>();
        }

        public Class(string name, Class baseClass)
        {
            Name = name;
            Base = baseClass;
            Scope = new Scope();
            Functions = new List<Function>();
        }

        public void AddFunction(Function function)
        {
            function.Scope.SetParent(Scope);
            Functions.Add(function);
        }

        public Function FindFunction(string name)
        {
            return Functions.FirstOrDefault(_ => _.Name == name);
        }
    }
}
