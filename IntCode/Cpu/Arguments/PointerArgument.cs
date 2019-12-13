using System;

namespace Cpu.Arguments
{
    public class PointerArgument<TType> : IArgument<TType>
    {
        private readonly IMemory<TType> _memory;

        private readonly int _addr;

        public PointerArgument(IMemory<TType> memory, int addr)
        {
            this._memory = memory ?? throw new ArgumentNullException(nameof(memory));
            this._addr = addr;
        }

        public TType Value
        {
            get
            {
                return this._memory[this._addr];
            }
            set
            {
                this._memory[this._addr] = value;
            }
        }
    }
}