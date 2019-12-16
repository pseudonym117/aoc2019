using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Cpu;
using Cpu.Devices;

namespace Day5
{
    class Day5Prog
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            await Part1(args[0]);
            await Part2(args[0]);
        }

        static async Task Part1(string filename)
        {
            var mem = await Util.ReadFileAsMemory(filename);

            var inputStream = new List<long> { 1 };
            var outputStream = new List<long>();

            var prog = new Program(mem, new ListDevice(inputStream), new ListDevice(outputStream));

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

            Console.WriteLine($"Part 1: {string.Join(", ", outputStream.Select(i => i.ToString()).ToArray())}");
        }

        static async Task Part2(string filename)
        {
            var mem = await Util.ReadFileAsMemory(filename);

            var inputStream = new List<long> { 5 };
            var outputStream = new List<long>();

            var prog = new Program(mem, new ListDevice(inputStream), new ListDevice(outputStream));

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

            Console.WriteLine($"Part 2: {string.Join(", ", outputStream.Select(i => i.ToString()).ToArray())}");
        }
    }
}
