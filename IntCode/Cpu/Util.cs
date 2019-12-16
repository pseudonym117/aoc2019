using System.Threading.Tasks;
using System.IO;
using System.Linq;

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
    }
}
