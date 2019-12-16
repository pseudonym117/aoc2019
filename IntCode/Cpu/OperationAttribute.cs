using System;

namespace Cpu
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class OperationAttribute : Attribute
    {
        public OperationAttribute(OpCode op, int args = 0)
        {
            this.Op = op;
            this.Args = args;
        }
        
        public int Args { get; }
        public OpCode Op { get; }
    }
}