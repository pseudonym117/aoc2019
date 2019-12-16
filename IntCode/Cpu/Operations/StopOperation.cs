using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.STOP)]
    public class StopOperation : IOperation
    {
        public Task Exec(IProgram prog, params IArgument[] args)
        {
            throw new StopExecutionException();
        }
    }
}
