﻿using System;

namespace EGScript.Objects
{
    public class OperatorException : Exception
    {
        public OperatorException(string _operator, string valueOfObj1, ObjectType typeOfObj1, ScriptObject obj2) : base($"Impossible '{_operator}' comparison? ([{typeOfObj1}] {valueOfObj1} {_operator} [{obj2.Type}] '{obj2.TypeName}')")
        {
        }
    }
}
