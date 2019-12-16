using System;

namespace Cpu.Arguments
{
    public class RelativeArgument : IArgument
    {
        private readonly IMemory _memory;

        private readonly long _offset;

        public RelativeArgument(IMemory memory, long offset)
        {
            this._memory = memory ?? throw new ArgumentNullException(nameof(memory));
            this._offset = offset;
        }

        public long Value
        {
            get => this._memory[this._memory.RelativeBase + this._offset];
            set => this._memory[this._memory.RelativeBase + this._offset] = value;
        }
    }
}
