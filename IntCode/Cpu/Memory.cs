using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpu
{
    public class Memory : IMemory
    {
        private readonly List<long> _code;

        public Memory(IEnumerable<long> initialState)
        {
            this._code = initialState?.ToList() ?? throw new ArgumentNullException(nameof(initialState));
        }

        public long RelativeBase { get; set; }

        public long InstructionPointer { get; set; }

        public long this[long key]
        {
            get
            {
                while(true)
                {
                    try
                    {
                        return this._code[(int)key];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        if (key < 0)
                        {
                            throw;
                        }

                        var atLeast = key - this._code.Count;
                        this._code.AddRange(Enumerable.Repeat(0L, Math.Max(100, (int)atLeast)));
                    }
                }
            }
            set
            {
                while(true)
                {
                    try
                    {
                        this._code[(int)key] = value;
                        return;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        if (key < 0)
                        {
                            throw;
                        }

                        var atLeast = key - this._code.Count;
                        this._code.AddRange(Enumerable.Repeat(0L, Math.Max(100, (int)atLeast)));
                    }
                }
            }
        }

        public object Clone()
        {
            var newMem = new Memory(this._code);
            newMem.InstructionPointer = this.InstructionPointer;
            newMem.RelativeBase = this.RelativeBase;

            return newMem;
        }
    }
}
