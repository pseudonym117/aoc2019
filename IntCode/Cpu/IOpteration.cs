using System.Threading.Tasks;

namespace Cpu
{
    public interface IOperation<TType>
    {
        Task Exec(params IArgument<TType>[] args);
    }
}