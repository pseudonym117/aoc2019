using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Cpu;
using Cpu.Devices;

namespace Day2
{
    class Day2Prog
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var mem = await Util.ReadFileAsMemory(args[0]);

            // part 1 modifications
            mem[1] = 12;
            mem[2] = 2;

            var prog = new Program(mem, new ListDevice(new List<long>()), new ListDevice(new List<long>()));

            try
            {
                while (true)
                {
                    await prog.Next();
                }
            }
            catch (StopExecutionException)
            {
            }

            Console.WriteLine($"Part 1: {prog.Memory[0]}");
        }
    }
}
