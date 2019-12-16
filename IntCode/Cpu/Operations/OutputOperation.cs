using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.OUTPUT, args: 1)]
    public class OutputOperation : IOperation
    {
        public Task Exec(IProgram prog, params IArgument[] args)
        {
            var value = args[0];

            return prog.Output.Put(value.Value);
        }
    }
}
