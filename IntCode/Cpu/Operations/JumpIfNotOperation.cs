using System;
using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.JMPIFN, args: 2)]
    public class JumpIfnotOperation<TType> : IOperation<TType> where TType : IComparable
    {
        public Task Exec(params IArgument<TType>[] args)
        {
            var compare = args[0];

            if (compare.Value.CompareTo(0) == 0)
            {
                var destination = args[1];

                // todo: figure out jump
            }

            return Task.CompletedTask;
        }
    }
}
