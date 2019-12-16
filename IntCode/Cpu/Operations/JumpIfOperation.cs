using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.JMPIF, args: 2)]
    public class JumpIfOperation : IOperation
    {
        public Task Exec(IProgram prog, params IArgument[] args)
        {
            var compare = args[0];

            if (compare.Value != 0)
            {
                var destination = args[1];

                prog.Memory.InstructionPointer = destination.Value;
            }

            return Task.CompletedTask;
        }
    }
}
