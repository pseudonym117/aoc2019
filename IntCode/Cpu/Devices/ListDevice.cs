using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpu.Devices
{
    public class ListDevice : IInputDevice, IOutputDevice
    {
        private readonly IList<long> _list;

        private int _index = 0;

        public ListDevice(IList<long> initialInput)
        {
            this._list = initialInput ?? throw new ArgumentNullException(nameof(initialInput));
        }

        public Task<long> Get()
        {
            var val = this._list[this._index];
            this._index++;

            return Task.FromResult(val);
        }

        public bool TryGet(out long value)
        {
            value = long.MinValue;
            if (this._index < this._list.Count)
            {
                value = this._list[this._index];
                this._index++;
                return true;
            }

            return false;
        }

        public Task Put(long value)
        {
            this._list.Add(value);

            return Task.CompletedTask;
        }
    }
}

