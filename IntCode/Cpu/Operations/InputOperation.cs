using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.INPUT, args: 1)]
    public class InputOperation : IOperation
    {
        public async Task Exec(IProgram prog, params IArgument[] args)
        {
            var destination = args[0];

            var val = await prog.Input.Get();

            destination.Value = val;
        }
    }
}
