using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpu
{
    public class Memory<TType> : IMemory<TType> where TType : struct
    {
        private readonly List<TType> _code;

        public Memory(IEnumerable<TType> initialState)
        {
            this._code = initialState?.ToList() ?? throw new ArgumentNullException(nameof(initialState));
        }

        public int RelativeBase { get; set; }

        public int InstructionPointer { get; set; }

        public TType this[int key]
        {
            get
            {
                while(true)
                {
                    try
                    {
                        return this._code[key];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        if (key < 0)
                        {
                            throw;
                        }

                        var atLeast = key - this._code.Count;
                        this._code.AddRange(Enumerable.Repeat(default(TType), Math.Max(100, atLeast)));
                    }
                }
            }
            set
            {
                while(true)
                {
                    try
                    {
                        this._code[key] = value;
                        return;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        if (key < 0)
                        {
                            throw;
                        }

                        var atLeast = key - this._code.Count;
                        this._code.AddRange(Enumerable.Repeat(default(TType), Math.Max(100, atLeast)));
                    }
                }
            }
        }
    }
}