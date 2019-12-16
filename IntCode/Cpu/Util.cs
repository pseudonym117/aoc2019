using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Cpu
{
    public static class Util
    {
        public static async Task<IMemory> ReadFileAsMemory(string filename)
        {
            var contents = await File.ReadAllTextAsync(filename);

            var code = contents.Split(',').Select(str => long.Parse(str));

            return new Memory(code);
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IList<T> list) => list.Permutations(list.Count);

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IList<T> list, int length)
        {
            if (length == 1)
            {
                return list.Select(t => new T[] { t });
            }

            return list
                .Permutations(length - 1)
                .SelectMany(
                    t => list.Where(
                        e => !t.Contains(e)
                    ),
                    (t1, t2) => t1.Concat(new T[] { t2 })
                );
        }

        public static T Time<T>(Func<T> func, [CallerMemberName] string? functionName = null)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                return func();
            }
            finally
            {
                Console.WriteLine($"perf {functionName!}: {stopwatch.ElapsedMilliseconds}ms");
            }
        }

        public static async Task<T> Time<T>(Func<Task<T>> func, [CallerMemberName] string? functionName = null)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                return await func();
            }
            finally
            {
                Console.WriteLine($"perf {functionName}: {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
}
