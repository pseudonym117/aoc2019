using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Linq;

using Cpu;
using Cpu.Devices;

namespace Day7
{
    class Day7Prog
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var initialMemory = await Util.ReadFileAsMemory(args[0]);

            var channels = Enumerable.Range(0, 5)
                .Select(_ => Channel.CreateUnbounded<long>())
                .ToArray();
            
            var devices = channels
                .Select(chan => new ChannelDevice(chan))
                .ToArray();

            var programs = new List<Program>();
            for (var i = 1; i < devices.Length; i++)
            {
                var prog = new Program((IMemory)initialMemory.Clone(), devices[i - 1], devices[i]);
                programs.Add(prog);
            }

            var tasks = programs
                .Select(prog => Task.Run(
                    async () => {
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
                    }
                )).ToArray();

            Task.WaitAll(tasks);

            Console.WriteLine($"Part 1: {prog.Memory[0]}");
        }
    }
}
