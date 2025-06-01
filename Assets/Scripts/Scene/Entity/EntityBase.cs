using System;
using System.Collections.Generic;
#nullable enable

public class EntityBase
{
    private Dictionary<string, string> stats;

    public EntityBase(EntityData entityData)
    {
        stats = new();
        foreach (var statEntry in entityData.statKeyWithValueArr)
        {
            stats.Add(statEntry.key, statEntry.value);
        }
    }
    private static class StatParser
    {
        private static readonly Dictionary<Type, object> strategies = new()
        {
            { typeof(float), new FloatParseStrategy() },
            { typeof(string), new StringParseStrategy() }
        };

        private static class StrategyCache<T>
        {
            public static readonly IStatParseStrategy<T>? cachedStrategy = null;

            static StrategyCache()
            {
                Type type = typeof(T);
                if (strategies.TryGetValue(type, out var strategy))
                {
                    if (strategy is IStatParseStrategy<T> _strategy)
                    {
                        cachedStrategy = _strategy;
                    }
                    else
                    {
                        Logger.LogError($"[StatParser] Stat Parse Strategy for {type} mismatched for IStatParseStrategy<{type}>");
                        cachedStrategy = null;
                    }
                }
                else
                {
                    Logger.LogError($"[StatParser] Stat Parse Strategy not found {type}");
                }
            }
        }

        public static IStatParseStrategy<T>? GetStrategy<T>()
        {
            return StrategyCache<T>.cachedStrategy;
        }
    }

    public bool TryGetStat<T>(string key, out T? value)
    {
        value = default;
        var strategy = StatParser.GetStrategy<T>();
        if (strategy != null && strategy.TryGetStat(stats, key, out var ret))
        {
            value = ret;
            return true;
        }
        return false;
    }

    public T? GetStat<T>(string key)
    {
        var strategy = StatParser.GetStrategy<T>();
        if (strategy != null)
        {
            return strategy.GetStat(stats, key);
        }

        return default;
    }

    public T? SetStat<T>(string key, T value)
    {
        var strategy = StatParser.GetStrategy<T>();
        if (strategy != null)
        {
            return strategy.SetStat(stats, key, value);
        }

        return default;
    }
}
