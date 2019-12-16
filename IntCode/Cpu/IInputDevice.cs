using System.Threading.Tasks;

namespace Cpu
{
    public interface IInputDevice
    {
        bool TryGet(out long value);

        Task<long> Get();
    }
}
