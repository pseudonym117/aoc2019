using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.MRB, args: 1)]
    public class ModifyRelativeBaseOperation : IOperation<long>
    {
        public Task Exec(params IArgument<long>[] args)
        {
            var modifier = args[0];

            // todo: implement

            return Task.CompletedTask;
        }
    }
}
