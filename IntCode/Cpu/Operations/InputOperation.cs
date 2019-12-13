using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.INPUT, args: 1)]
    public class InputOperation<TType> : IOperation<TType>
    {
        public async Task Exec(params IArgument<TType>[] args)
        {
            var destination = args[0];

            // todo: find a way to link up IO
        }
    }
}
