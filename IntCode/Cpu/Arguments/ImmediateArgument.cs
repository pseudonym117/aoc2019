using System;

namespace Cpu.Arguments
{
    public class ImmediateArgument : IArgument
    {
        private readonly long _value;

        public ImmediateArgument(long value)
        {
            this._value = value;
        }

        public long Value
        {
            get
            {
                return this._value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
