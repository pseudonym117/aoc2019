using System;

namespace Cpu.Arguments
{
    public class PointerArgument : IArgument
    {
        private readonly IMemory _memory;

        public PointerArgument(IMemory memory)
        {
            this._memory = memory ?? throw new ArgumentNullException(nameof(memory));
        }

        public long Raw { get; set; }

        public long Value
        {
            get
            {
                return this._memory[this.Raw];
            }
            set
            {
                this._memory[this.Raw] = value;
            }
        }
    }
}