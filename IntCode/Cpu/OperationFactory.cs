using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpu
{
    public class OperationFactory
    {
        private readonly IDictionary<OpCode, (IOperation, int)> _opMap;

        public OperationFactory()
        {
            this._opMap = OperationFactory.InitializeMap();
        }

        public (IOperation, int) Get(OpCode op) => this._opMap[op];

        private static IDictionary<OpCode, (IOperation, int)> InitializeMap()
        {
            var res = new Dictionary<OpCode, (IOperation, int)>();
            foreach (var type in typeof(OperationFactory).Module.GetTypes())
            {
                if (!type.GetInterfaces().Contains(typeof(IOperation)))
                {
                    continue;
                }

                var attrs = type.GetCustomAttributes(typeof(OperationAttribute), true);

                if (attrs.Length == 0)
                {
                    continue;
                }

                var attr = (OperationAttribute)attrs.First();

                var typeOp = attr.Op;
                var argCount = attr.Args;

                var constructor = type.GetConstructor(Type.EmptyTypes);

                if (constructor == null)
                {
                    throw new MissingMethodException($"missing default constructor for type {type}");
                }

                res[typeOp] = ((IOperation)constructor.Invoke(null), argCount);
            }

            return res;
        }
    }
}
