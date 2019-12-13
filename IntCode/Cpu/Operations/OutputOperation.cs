using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.OUTPUT, args: 1)]
    public class OutputOperation<TType> : IOperation<TType>
    {
        public async Task Exec(params IArgument<TType>[] args)
        {
            var value = args[0];

            // todo: link up IO
        }
    }
}
