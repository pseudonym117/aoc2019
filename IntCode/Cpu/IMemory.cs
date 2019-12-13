
namespace Cpu
{
    public interface IMemory<TType>
    {
        int RelativeBase { get; set; }

        int InstructionPointer { get; set; }

        TType this[int key] { get; set; }
    }
}
