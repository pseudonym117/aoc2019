using System.Threading.Tasks;

namespace Cpu
{
    public interface IOutputDevice
    {
        Task Put(long value);
    }
}
