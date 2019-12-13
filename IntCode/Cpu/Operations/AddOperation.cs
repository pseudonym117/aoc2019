using System.Threading.Tasks;

namespace Cpu.Operations
{
    [Operation(OpCode.ADD, args: 3)]
    public class AddOperation : IOperation<long>
    {
        public Task Exec(params IArgument<long>[] args)
        {
            var operand1 = args[0];
            var operand2 = args[1];
            var destination = args[3];

            destination.Value = operand1.Value + operand2.Value;

            return Task.CompletedTask;
        }
    }
}
