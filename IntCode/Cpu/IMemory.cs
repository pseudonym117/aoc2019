using System;

namespace Cpu
{
    public interface IMemory : ICloneable
    {
        long RelativeBase { get; set; }

        long InstructionPointer { get; set; }

        long this[long key] { get; set; }
    }
}
