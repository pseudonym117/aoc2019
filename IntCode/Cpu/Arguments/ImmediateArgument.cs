using System;

namespace Cpu.Arguments
{
    public class ImmediateArgument : IArgument
    {
        public long Raw { get; set; }

        public long Value
        {
            get
            {
                return this.Raw;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
