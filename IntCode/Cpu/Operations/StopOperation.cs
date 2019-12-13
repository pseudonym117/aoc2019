using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.STOP)]
    public class StopOperation<TType> : IOperation<TType>
    {
        public Task Exec(params IArgument<TType>[] args)
        {
            throw new StopExecutionException();
        }
    }
}
