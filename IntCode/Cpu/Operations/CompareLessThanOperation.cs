using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.CLT, args: 3)]
    public class CompareLessThanOperation : IOperation
    {
        public Task Exec(IProgram prog, params IArgument[] args)
        {
            var operand1 = args[0];
            var operand2 = args[1];
            var destination = args[2];

            if (operand1.Value < operand2.Value)
            { 
                destination.Value = 1;
            }
            else 
            {
                destination.Value = 0;
            }

            return Task.CompletedTask;
        }
    }
}
