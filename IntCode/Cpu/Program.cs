using System;
using System.Threading.Tasks;
using System.Linq;

using Cpu.Operations;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using Cpu.Arguments;

namespace Cpu
{
    public class Program : IProgram
    {
        private readonly IDictionary<OpCode, (Func<IOperation>, int)> _typeMapper = new Dictionary<OpCode, (Func<IOperation>, int)>();

        public IMemory Memory { get; }

        public IInputDevice Input { get; }

        public IOutputDevice Output { get; }

        public Program(IMemory memory, IInputDevice input, IOutputDevice output)
        {
            this.Memory = memory ?? throw new ArgumentNullException(nameof(memory));
            this.Input = input;
            this.Output = output;

            this.InitializeTypeMapper();
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

            var (ctor, argsCount) = this._typeMapper[op];

            var operationObj = ctor();

            var args = Enumerable.Range(2, argsCount)
                .Select(i => this.GetNextArgument((ArgumentMode)((opCodeRaw / ((long)Math.Pow(10, i))) % 10)))
                .ToArray();

            return (operationObj, args);
        }

        private IArgument GetNextArgument(ArgumentMode mode) => mode switch
        {
            ArgumentMode.POINTER => new PointerArgument(this.Memory, this.Memory[this.Memory.InstructionPointer++]),
            ArgumentMode.IMMEDIATE => new ImmediateArgument(this.Memory[this.Memory.InstructionPointer++]),
            ArgumentMode.RELATIVE => new RelativeArgument(this.Memory, this.Memory[this.Memory.InstructionPointer++]),
            _ => throw new ArgumentException("invalid mode", nameof(mode))
        };

        private void InitializeTypeMapper()
        {
            foreach (var type in typeof(Program).Module.GetTypes())
            {
                if (!type.GetInterfaces().Contains(typeof(IOperation)))
                {
                    continue;
                }

                var attrs = type.GetCustomAttributes(typeof(OperationAttribute), true);

                if (attrs.Length == 0)
                {
                    continue;
                }

                var attr = (OperationAttribute)attrs.First();

                var typeOp = attr.Op;
                var argCount = attr.Args;

                var constructor = type.GetConstructor(Type.EmptyTypes);

                if (constructor == null)
                {
                    throw new MissingMethodException($"missing default constructor for type {type}");
                }

                var constructExpr = Expression.Lambda<Func<IOperation>>(Expression.New(constructor)).Compile();
                this._typeMapper[typeOp] = (constructExpr, argCount);
            }
        }
    }
}
