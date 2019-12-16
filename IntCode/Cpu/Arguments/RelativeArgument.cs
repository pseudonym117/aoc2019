using System;

namespace Cpu.Arguments
{
    public class RelativeArgument : IArgument
    {
        private readonly IMemory _memory;

        public RelativeArgument(IMemory memory)
        {
            this._memory = memory ?? throw new ArgumentNullException(nameof(memory));
        }

        public long Raw { get; set; }

        public long Value
        {
            get => this._memory[this._memory.RelativeBase + this.Raw];
            set => this._memory[this._memory.RelativeBase + this.Raw] = value;
        }
    }
}
