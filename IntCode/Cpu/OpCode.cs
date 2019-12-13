namespace Cpu
{
    public enum OpCode : long
    {
        ADD = 1,
        MULT = 2,
        INPUT = 3,
        OUTPUT = 4,
        JMPIF = 5,
        JMPIFN = 6,
        CLT = 7,
        CEQ = 8,
        MRB = 9,
        STOP = 99,
    }
}
