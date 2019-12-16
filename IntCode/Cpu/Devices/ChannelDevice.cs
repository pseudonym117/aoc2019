using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Cpu.Devices
{
    public class ChannelDevice : IInputDevice, IOutputDevice
    {
        private readonly Channel<long> _channel;

        public ChannelDevice(Channel<long> channel)
        {
            this._channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        public async Task<long> Get()
        {
            return await this._channel.Reader.ReadAsync();
        }

        public bool TryGet(out long value)
        {
            return this._channel.Reader.TryRead(out value);
        }

        public async Task Put(long value)
        {
            await this._channel.Writer.WriteAsync(value);
        }
    }
}
