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

            await Part1(initialMemory);
            await Part2(initialMemory);
        }

        static async Task Part1(IMemory initialMemory)
        {
            var phases = Enumerable.Range(0, 5).ToList();

            var maxOutput = long.MinValue;

            foreach (var phaseConfig in phases.Permutations())
            {
                var channels = Enumerable.Range(0, 6)
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

                await Task.WhenAll(
                    devices
                        .Zip(phaseConfig)
                        .Select(pair => pair.First.Put(pair.Second))
                        .ToArray()
                );

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
                    ))
                    .ToArray();

                await devices.First().Put(0);

                await Task.WhenAll(tasks);

                var power = await devices.Last().Get();
                if (power > maxOutput)
                {
                    maxOutput = power;
                }
            }

            Console.WriteLine($"Part 1: max power is {maxOutput}");
        }

        static async Task Part2(IMemory initialMemory)
        {
            var phases = Enumerable.Range(5, 5).ToList();

            var maxOutput = long.MinValue;

            foreach (var phaseConfig in phases.Permutations())
            {
                var channels = Enumerable.Range(0, 5)
                    .Select(_ => Channel.CreateUnbounded<long>())
                    .ToArray();
                
                var devices = channels
                    .Select(chan => new ChannelDevice(chan))
                    .ToArray();

                var programs = new List<Program>();
                for (var i = 1; i <= devices.Length; i++)
                {
                    var prog = new Program((IMemory)initialMemory.Clone(), devices[i - 1], devices[i % devices.Length]);
                    programs.Add(prog);
                }

                await Task.WhenAll(
                    devices
                        .Zip(phaseConfig)
                        .Select(pair => pair.First.Put(pair.Second))
                        .ToArray()
                );

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
                    ))
                    .ToArray();

                await devices.First().Put(0);

                await Task.WhenAll(tasks);

                var power = await devices.First().Get();
                if (power > maxOutput)
                {
                    maxOutput = power;
                }
            }

            Console.WriteLine($"Part 2: max power is {maxOutput}");
        }
    }
}
