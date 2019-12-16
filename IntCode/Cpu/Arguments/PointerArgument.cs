using System;

namespace Cpu.Arguments
{
    public class PointerArgument : IArgument
    {
        private readonly IMemory _memory;

        private readonly long _addr;

        public PointerArgument(IMemory memory, long addr)
        {
            this._memory = memory ?? throw new ArgumentNullException(nameof(memory));
            this._addr = addr;
        }

        public long Value
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