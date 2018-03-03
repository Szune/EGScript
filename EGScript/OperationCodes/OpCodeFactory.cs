using EGScript.Objects;

namespace EGScript.OperationCodes
{
    public class OpCodeFactory
    {
        public static Return Return => new Return(); // return value will not be reliable if object is reused without resetting return value

        public static readonly Pop Pop = new Pop();

        public static readonly EqualsEquals EqualsEquals = new EqualsEquals();

        public static readonly Add Add = new Add();

        public static readonly Subtract Subtract = new Subtract();

        public static readonly Multiply Multiply = new Multiply();

        public static readonly Divide Divide = new Divide();

        public static readonly Or Or = new Or();

        public static readonly And And = new And();

        public static readonly NotEquals NotEquals = new NotEquals();

        public static readonly LessThan LessThan = new LessThan();

        public static readonly GreaterThan GreaterThan = new GreaterThan();

        public static readonly LessThanEquals LessThanEquals = new LessThanEquals();

        public static readonly GreaterThanEquals GreaterThanEquals = new GreaterThanEquals();

        public static readonly Negate Negate = new Negate();

        public static readonly Not Not = new Not();

        public static readonly Increment Increment = new Increment();

        public static readonly Decrement Decrement = new Decrement();

        public static readonly MakeTable MakeTable = new MakeTable();

        public static readonly Count Count = new Count();

        public static Set Set(StringObj variableName)
        {
            return new Set(variableName);
        }

        public static Push Push(ScriptObject scriptObject)
        {
            return new Push(scriptObject);
        }

        public static BranchIfFalse BranchIfFalse(uint argument)
        {
            return new BranchIfFalse(argument);
        }

        public static Branch Branch(uint condition)
        {
            return new Branch(condition);
        }

        public static Define Define(StringObj variableName)
        {
            return new Define(variableName);
        }

        public static Reference Reference(StringObj variableName)
        {
            return new Reference(variableName);
        }

        public static BranchIfTrue BranchIfTrue(uint argument)
        {
            return new BranchIfTrue(argument);
        }

        public static MemberFunctionCall MemberFunctionCall(StringObj functionName)
        {
            return new MemberFunctionCall(functionName);
        }

        public static ExportedFunctionCall ExportedFunctionCall(StringObj functionName)
        {
            return new ExportedFunctionCall(functionName);
        }

        public static FunctionCall FunctionCall(StringObj functionName)
        {
            return new FunctionCall(functionName);
        }

        public static Reference Reference(StringObj variableName, uint argument)
        {
            return new Reference(variableName, argument);
        }

        public static Global Global(StringObj variableName)
        {
            return new Global(variableName);
        }
    }
}
