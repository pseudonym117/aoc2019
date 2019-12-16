using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Cpu;
using Cpu.Devices;

namespace Day9
{
    class Day9Prog
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var mem = await Util.ReadFileAsMemory(args[0]);

            await Util.Time(() => Part1((IMemory)mem.Clone()), "Part 1");
            await Util.Time(() => Part2(mem), "Part 2");
        }

        static async Task Part1(IMemory mem)
        {
            var input = new ListDevice(new List<long>{ 1 });
            var output = new ListDevice(new List<long>());
            var prog = new Program(mem, input, output);

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

            Console.WriteLine($"Part 1: {await output.Get()}");
        }

        static async Task Part2(IMemory mem)
        {
            var input = new ListDevice(new List<long>{ 2 });
            var output = new ListDevice(new List<long>());
            var prog = new Program(mem, input, output);

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

            Console.WriteLine($"Part 2: {await output.Get()}");
        }
    }
}
