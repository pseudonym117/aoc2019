using System.Threading.Tasks;

namespace Cpu
{
    public interface IProgram
    {
        IMemory Memory { get; }

        IInputDevice Input { get; }

        IOutputDevice Output { get; }

        Task Next();
    }
}
