using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Cpu
{
    public class Program
    {
        private readonly Memory<long> _code;

        private BlockingCollection<long> _input;

        private BlockingCollection<long> _output;

        public long RelativeBase { get; internal set; } = 0;

        public long InstructionPointer { get; internal set; } = 0;

        public Program(IList<long> code, BlockingCollection<long> input, BlockingCollection<long> output)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            
            this._code = new Memory<long>(code);
            this._input = input ?? throw new ArgumentNullException(nameof(input));
            this._output = output ?? throw new ArgumentNullException(nameof(output));
        }
    }
}