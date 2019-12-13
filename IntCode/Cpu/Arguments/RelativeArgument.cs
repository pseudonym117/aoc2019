using System;

namespace Cpu.Arguments
{
    public class RelativeArgument<TType> : IArgument<TType>
    {
        private readonly IMemory<TType> _memory;

        private readonly int _offset;

        public RelativeArgument(IMemory<TType> memory, int offset)
        {
            this._memory = memory ?? throw new ArgumentNullException(nameof(memory));
            this._offset = offset;
        }

        public TType Value
        {
            get => this._memory[this._memory.RelativeBase + this._offset];
            set => this._memory[this._memory.RelativeBase + this._offset] = value;
        }
    }
}
