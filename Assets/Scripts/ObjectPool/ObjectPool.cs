using System.Collections.Generic;
using System;
using UnityEngine;
#nullable enable

public class ObjectPool
{

    private readonly Dictionary<Type, object> strategies = new();
    private static class StrategyCache<T>
    {

        public static IManagePoolStrategy<T>? cachedStrategy = null;
    }

    public void RegisterStrategy<T>(IManagePoolStrategy<T> strategy)
    {
        var type = typeof(T);
        if (strategies.ContainsKey(type))
        {
            Logger.LogWarning($"[ObjectPool] Strategy for {type} already registered. Overwriting.");
        }
        strategies[type] = strategy;
        StrategyCache<T>.cachedStrategy = strategy;
    }

    public bool TryGet<T>(out T result)
    {
        if (StrategyCache<T>.cachedStrategy != null)
        {
            result = StrategyCache<T>.cachedStrategy.Get();
            return true;
        }
        result = default!;
        return false;
    }

    public T? Get<T>()
    {
        if (StrategyCache<T>.cachedStrategy != null)
        {
            return StrategyCache<T>.cachedStrategy.Get();
        }
        return default;
    }

    public void Return<T>(T obj)
    {
        if (StrategyCache<T>.cachedStrategy != null)
        {
            StrategyCache<T>.cachedStrategy.Return(obj!);
        }
    }
}