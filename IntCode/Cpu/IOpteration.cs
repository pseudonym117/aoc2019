using System.Threading.Tasks;

namespace Cpu
{
    public interface IOperation
    {
        Task Exec(IProgram prog, params IArgument[] args);
    }
}
