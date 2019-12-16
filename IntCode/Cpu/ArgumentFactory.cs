using System;
using System.Collections.Generic;
using System.Linq;

using Cpu.Arguments;

namespace Cpu
{
    public class ArgumentFactory
    {
        private readonly IMemory _mem;

        private readonly IDictionary<ArgumentMode, Pool<IArgument>> _pools;

        public ArgumentFactory(IMemory mem)
        {
            this._mem = mem ?? throw new ArgumentNullException(nameof(mem));
            this._pools = this.InitPools();
        }

        public IArgument Get(ArgumentMode mode, long value)
        {
            var arg = this._pools[mode].Next();
            arg.Raw = value;

            return arg;
        }

        private IDictionary<ArgumentMode, Pool<IArgument>> InitPools()
        {
            var maxArgs = ArgumentFactory.MaxArgs();
            
            return new Dictionary<ArgumentMode, Pool<IArgument>>
            {
                { ArgumentMode.POINTER, new Pool<IArgument>(() => new PointerArgument(this._mem), maxArgs) },
                { ArgumentMode.IMMEDIATE, new Pool<IArgument>(() => new ImmediateArgument(), maxArgs) },
                { ArgumentMode.RELATIVE, new Pool<IArgument>(() => new RelativeArgument(this._mem), maxArgs) },
            };
        }

        private static int MaxArgs()
        {
            var max = int.MinValue;
            foreach (var type in typeof(OperationFactory).Module.GetTypes())
            {
                if (!type.GetInterfaces().Contains(typeof(IOperation)))
                {
                    continue;
                }

                var attrs = type.GetCustomAttributes(typeof(OperationAttribute), true);

                if (attrs.Length == 0)
                {
                    continue;
                }

                var attr = (OperationAttribute)attrs.First();

                max = Math.Max(max, attr.Args);
            }

            return max;
        }
    }
}
