namespace Cpu
{
    public interface IArgument<TType>
    {
        TType Value { get; set; }
    }
}