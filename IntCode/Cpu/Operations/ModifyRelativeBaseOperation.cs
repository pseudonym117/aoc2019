using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.MRB, args: 1)]
    public class ModifyRelativeBaseOperation : IOperation
    {
        public Task Exec(IProgram prog, params IArgument[] args)
        {
            var modifier = args[0];

            prog.Memory.RelativeBase += modifier.Value;

            return Task.CompletedTask;
        }
    }
}
