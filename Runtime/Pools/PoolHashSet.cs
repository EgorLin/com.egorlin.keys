using System;
using System.Collections.Generic;

namespace EgorLin.Keys.Pools
{
#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    internal static class PoolHashSet<T>
    {
        private static readonly PoolInternalBase Pool = new(() => new HashSet<T>(),
            obj => ((HashSet<T>)obj).Clear());

        public static PooledHashSet<T> Spawn()
        {
            return new PooledHashSet<T>((HashSet<T>) Pool.Spawn());
        }

        public static void Recycle(ref HashSet<T> instance)
        {
            Pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(HashSet<T> instance)
        {
            Pool.Recycle(instance);
        }
    }

    public readonly struct PooledHashSet<T> : IDisposable
    {
        public readonly HashSet<T> Value;

        public PooledHashSet(HashSet<T> value)
        {
            Value = value;
        }

        public void Dispose()
        {
            if (Value != null)
            {
                PoolHashSet<T>.Recycle(Value);
            }
        }

        public static implicit operator HashSet<T>(PooledHashSet<T> pooled)
        {
            return pooled.Value;
        }

        public HashSet<T>.Enumerator GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public bool Add(T item)
        {
            return Value.Add(item);
        }

        public void Remove(T item)
        {
            Value.Remove(item);
        }

        public void Clear()
        {
            Value.Clear();
        }

        public bool Contains(T item)
        {
            return Value.Contains(item);
        }

        public void AddRange(IEnumerable<T> group)
        {
            foreach (T item in group)
            {
                Value.Add(item);
            }
        }
    }
}
