using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EgorLin.Keys.Pools
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    internal class PoolInternalBaseNoStackPool
    {
        public static int NewAllocated;
        public static int Allocated;
        public static int Deallocated;
        public static int Used;

        private static readonly List<PoolInternalBaseNoStackPool> List = new();

        protected Stack<object> Cache = new();
        protected Func<object> Constructor;
        protected Action<object> Desctructor;
        protected int PoolAllocated;
        protected int PoolDeallocated;
        protected int PoolNewAllocated;
        protected int PoolUsed;

        public PoolInternalBaseNoStackPool(Func<object> constructor, Action<object> desctructor)
        {
            Constructor = constructor;
            Desctructor = desctructor;

            List.Add(this);
        }

        public override string ToString()
        {
            return "Allocated: " + PoolAllocated + ", Deallocated: " + PoolDeallocated + ", Used: " + PoolUsed +
                   ", cached: " + Cache.Count + ", new: " + PoolNewAllocated;
        }

        public static void Clear()
        {
            List<PoolInternalBaseNoStackPool> pools = List;
            for (var i = 0; i < pools.Count; ++i)
            {
                PoolInternalBaseNoStackPool pool = pools[i];
                pool.Cache.Clear();
                pool.Constructor = null;
                pool.Desctructor = null;
            }

            pools.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Prewarm(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                Recycle(Spawn());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual object Spawn()
        {
            object item = null;
            if (Cache.Count > 0)
            {
                item = Cache.Pop();
            }

            if (item == null)
            {
                ++NewAllocated;
                ++PoolNewAllocated;
            }
            else
            {
                ++Used;
                ++PoolUsed;
            }

            if (Constructor != null && item == null)
            {
                item = Constructor.Invoke();
            }

            ++PoolAllocated;
            ++Allocated;

            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Recycle(object instance)
        {
            ++PoolDeallocated;
            ++Deallocated;

            if (Desctructor != null)
            {
                Desctructor.Invoke(instance);
            }

            Cache.Push(instance);
        }
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif

    public class PoolInternalBase
    {
#if MULTITHREAD_SUPPORT
	    protected CCStack<object> cache = new CCStack<object>(usePool: true);
#else
        public Stack<object> Cache = new();
#endif
        protected Func<object> Constructor;
        protected Action<object> Desctructor;

#if UNITY_EDITOR
        public Type PoolType;
        public string poolName => PoolType.Name;

        public int cacheCount => Cache.Count;
#endif

        public int NewAllocatedCurrent;

        public int SpawnedTotal;

        public int DespawnedTotal;
        public int TimesUsedFromCache;

        public int NewAllocatedTotal;

        public static readonly List<PoolInternalBase> List = new();

        public static int StaticNewAllocated;
        public static int StaticAllocated;
        public static int StaticDeallocated;
        public static int StaticUsed;

        public static void ClearCaches()
        {
            List<PoolInternalBase> pools = List;

            for (var i = 0; i < pools.Count; ++i)
            {
                PoolInternalBase pool = pools[i];
                pool.ClearCache();
            }
        }

        private void ClearCache()
        {
            NewAllocatedCurrent -= Cache.Count;
            Cache.Clear();
        }

        public void ResetStat()
        {
            SpawnedTotal = 0;
            DespawnedTotal = 0;
            TimesUsedFromCache = 0;
            NewAllocatedTotal = 0;
            NewAllocatedCurrent = 0;
        }

        public override string ToString()
        {
            return "Allocated: " + SpawnedTotal + ", Deallocated: " + DespawnedTotal + ", Used: " + TimesUsedFromCache +
                   ", cached: " + Cache.Count + ", new: " + NewAllocatedTotal;
        }

        public PoolInternalBase(Func<object> constructor, Action<object> desctructor)
        {
            Constructor = constructor;
            Desctructor = desctructor;

            List.Add(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Prewarm(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                Recycle(Spawn());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual object Spawn()
        {
#if MULTITHREAD_SUPPORT
		    this.cache.TryPop(out object item);
#else
            object item = Cache.Count > 0 ? Cache.Pop() : null;
#endif

            if (item == null)
            {
                ++StaticNewAllocated;
                ++NewAllocatedTotal;
                ++NewAllocatedCurrent;
            }
            else
            {
                ++StaticUsed;
                ++TimesUsedFromCache;
            }

            if (Constructor != null && item == null)
            {
                item = Constructor.Invoke();

#if UNITY_EDITOR
                if (item != null)
                {
                    PoolType = item.GetType();
                }
#endif

#if ENABLE_LOG_POOL_ALLOCATE_NEW && UNITY_EDITOR
                Debug.Log($"#Pool# {poolName} allocated new object");
#endif
            }

            ++SpawnedTotal;
            ++StaticAllocated;

            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Recycle(object instance)
        {
#if UNITY_EDITOR
            if (instance != null)
            {
                PoolType = instance.GetType();
            }
#endif

            ++DespawnedTotal;
            ++StaticDeallocated;

            if (Desctructor != null)
            {
                Desctructor.Invoke(instance);
            }

            Cache.Push(instance);
        }
    }
}