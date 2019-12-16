using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.MULT, args: 3)]
    public class MultiplyOperation : IOperation
    {
        public Task Exec(IProgram prog, params IArgument[] args)
        {
            var operand1 = args[0];
            var operand2 = args[1];
            var destination = args[2];

            destination.Value = operand1.Value * operand2.Value;

            return Task.CompletedTask;
        }
    }
}
