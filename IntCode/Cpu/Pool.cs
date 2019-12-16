using System;
using System.Linq;

namespace Cpu
{
    internal class Pool<T>
    {
        private readonly T[] _objs;

        private int _next;

        public Pool(Func<T> createFunc, int maxObjects)
        {
            this._objs = Enumerable
                .Range(0, maxObjects)
                .Select(_ => createFunc())
                .ToArray();
        }

        public T Next()
        {
            var res = _objs[this._next++];
            this._next %= _objs.Length;
            return res;
        }
    }
}