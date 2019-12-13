using System;

namespace Cpu.Arguments
{
    public class ImmediateArgument<TType> : IArgument<TType>
    {
        private readonly TType _value;

        public ImmediateArgument(TType value)
        {
            this._value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public TType Value
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
