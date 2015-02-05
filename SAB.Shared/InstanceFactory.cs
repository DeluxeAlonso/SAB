using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Shared
{
    public sealed class InstanceFactory
    {
        public static InstanceFactory Instance = new InstanceFactory();

        private readonly Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();

        private InstanceFactory() { }

        public void Register(Type typeA, Type typeB)
        {
            typeMap.Add(typeA, typeB);
        }

        public void Register(Type typeA)
        {
            typeMap.Add(typeA, typeA);
        }

        public T GetInstance<T>()
        {
            if (typeMap.ContainsKey(typeof(T)))
            {
                if (instances.ContainsKey(typeof(T)))
                {
                    return (T)instances[typeof(T)];
                }
                else
                {
                    var instance = Activator.CreateInstance(typeMap[typeof(T)]);
                    instances.Add(typeof(T), instance);
                    return (T)instance;
                }
            }
            else
            {
                throw new Exception("El tipo no se encuentra mapeado.");
            }
        }

        public T GetInstance<T>(Type type)
        {
            if (typeMap.ContainsKey(type))
            {
                if (instances.ContainsKey(type))
                {
                    return (T)instances[type];
                }
                else
                {
                    var instance = Activator.CreateInstance(typeMap[type]);
                    instances.Add(type, instance);
                    return (T)instance;
                }
            }
            else
            {
                throw new Exception("El tipo no se encuentra mapeado.");
            }
        }
    }
}
