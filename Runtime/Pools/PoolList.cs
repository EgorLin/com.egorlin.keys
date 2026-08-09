using System;
using System.Collections.Generic;

namespace EgorLin.Keys.Pools
{
#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    internal static class PoolList<T>
    {
        private static readonly PoolInternalBase Pool = new(() => new List<T>(), 
            obj => ((List<T>)obj).Clear());

        public static PooledList<T> Spawn()
        {
            return new PooledList<T>((List<T>) Pool.Spawn());
        }

        public static void Recycle(ref List<T> instance)
        {
            Pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(List<T> instance)
        {
            Pool.Recycle(instance);
        }
    }

    public readonly struct PooledList<TItem> : IDisposable
    {
        public readonly List<TItem> Value;

        public PooledList(List<TItem> value)
        {
            Value = value;
        }

        public TItem this[int index]
        {
            get => Value[index];
            set => Value[index] = value;
        }

        public int Count => Value.Count;

        public void Dispose()
        {
            if (Value != null)
            {
                PoolList<TItem>.Recycle(Value);
            }
        }

        public static implicit operator List<TItem>(PooledList<TItem> pooled)
        {
            return pooled.Value;
        }

        public List<TItem>.Enumerator GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            Value.AddRange(items);
        }

        public void Sort(IComparer<TItem> comparer)
        {
            Value.Sort(comparer);
        }

        public void Sort(Comparison<TItem> comparison)
        {
            Value.Sort(comparison);
        }

        public void Clear()
        {
            Value.Clear();
        }

        public void Add(TItem item)
        {
            Value.Add(item);
        }

        public void RemoveAt(int i)
        {
            Value.RemoveAt(i);
        }
    }
}
