using System.Threading.Tasks;

namespace Cpu
{
    public interface IInputDevice
    {
        Task<long> Get();
    }
}
