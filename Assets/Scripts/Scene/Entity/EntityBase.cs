using System;
using System.Collections.Generic;
using UnityEditor;
#nullable enable

public class EntityBase
{
    private Dictionary<string, string> stats;
    private Dictionary<Type, object> statParseStrategies; // 기존 전략 저장소

    public EntityBase(EntityData entityData)
    {
        stats = new();
        // todo stat type과 id를 매핑해서 검사해야하지만 나중에 구현하고 지금은 json내 string 파일 그대로 저장
        foreach (var statEntry in entityData.statKeyWithValueArr)
        {
            stats.Add(statEntry.key, statEntry.value);
        }
        statParseStrategies = new();
        statParseStrategies[typeof(float)] = new FloatParseStrategy();
        statParseStrategies[typeof(string)] = new StringParseStrategy();
    }

    // 제너릭 타입마다 캐시된 전략을 static으로 유지
    private static class StatParseCache<T>
    {
        public static IStatParseStrategy<T>? cachedStrategy = null;
    }

    private IStatParseStrategy<T>? GetStatParseStrategy<T>()
    {
        if (StatParseCache<T>.cachedStrategy != null)
        {
            return StatParseCache<T>.cachedStrategy;
        }
        var type = typeof(T);
        if (statParseStrategies.ContainsKey(type))
        {
            var strategy = statParseStrategies[type];
            if (strategy is IStatParseStrategy<T> typedStrategy)
            {
                StatParseCache<T>.cachedStrategy = typedStrategy;
                return typedStrategy;
            }
        }

        Logger.LogError($"[EntityBase] Stat parse Strategy for {type} not found or does not match IStatParseStrategy<{type}>");
        return null;
    }

    public bool TryGetStat<T>(string key, out T? value)
    {
        value = default;
        var strategy = GetStatParseStrategy<T>();

        if (strategy != null && strategy.TryGetStat(stats, key, out var ret))
        {
            value = ret;
            return true;
        }

        return false;
    }

    public T? GetStat<T>(string key)
    {
        var strategy = GetStatParseStrategy<T>();

        if (strategy != null)
        {
            return strategy.GetStat(stats, key);
        }

        return default;
    }

    public T? SetStat<T>(string key, T value)
    {
        var strategy = GetStatParseStrategy<T>();

        if (strategy != null)
        {
            return strategy.SetStat(stats, key, value);
        }

        return default;
    }
}
