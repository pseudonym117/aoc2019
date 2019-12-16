using System;
using System.Threading.Tasks;
using System.Linq;

using Cpu.Arguments;

namespace Cpu
{
    public class Program : IProgram
    {
        private readonly OperationFactory _opFactory = new OperationFactory();

        private readonly ArgumentFactory _argFactory;

        public IMemory Memory { get; }

        public IInputDevice Input { get; }

        public IOutputDevice Output { get; }

        public Program(IMemory memory, IInputDevice input, IOutputDevice output)
        {
            this.Memory = memory ?? throw new ArgumentNullException(nameof(memory));
            this.Input = input;
            this.Output = output;

            this._argFactory = new ArgumentFactory(this.Memory);
        }

        public Task Next()
        {
            var (operation, args) = this.GetNextOperation();

            return operation.Exec(this, args);
        }

        private (IOperation, IArgument[]) GetNextOperation()
        {
            var opCodeRaw = this.Memory[this.Memory.InstructionPointer++];

            var op = (OpCode)(opCodeRaw % 100);

            var (operationObj, argsCount) = this._opFactory.Get(op);

            var args = Enumerable.Range(2, argsCount)
                .Select(i => this.GetNextArgument((ArgumentMode)((opCodeRaw / ((long)Math.Pow(10, i))) % 10)))
                .ToArray();

            return (operationObj, args);
        }

        private IArgument GetNextArgument(ArgumentMode mode) => this._argFactory.Get(mode, this.Memory[this.Memory.InstructionPointer++]);
    }
}
