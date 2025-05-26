using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
#nullable enable

public class EntityBase
{
    private Dictionary<string, object> stats;
    private Dictionary<Type, object> statParseStrategies;
    public EntityBase(EntityData entityData)
    {
        stats = new();
        // todo stat type과 id를 매핑해서 검사해야하지만 나중에 구현하고 지금은 json내 string 파일 그대로 저장
        foreach(var statEntry in entityData.statKeyWithValueArr)
        {
            stats.Add(statEntry.key, statEntry.value);
        }
        statParseStrategies = new();
        statParseStrategies[typeof(float)] = new FloatParseStrategy();
        statParseStrategies[typeof(string)] = new StringParseStrategy();
    }
    public bool TryGetStat<T>(string key, out T? value)
    {
        value = default;
        if (statParseStrategies.ContainsKey(typeof(T)))
        {
            var strategy = statParseStrategies[typeof(T)];
            if (strategy is IStatParseStrategy<T>)
            {
                if (((IStatParseStrategy<T>)strategy).TryGetStat(stats, key, out var ret))
                {
                    value = ret;
                    return true;
                }
                return false;
            }
            else
            {
                Logger.LogError($"[EntityBase] Stat parse Strategy for {typeof(T)} not match for IStatParseStrategy<{typeof(T)}>");
                return false;
            }
        }
        else
        {
            Logger.LogError($"[EntityBase] Stat parse Strategy not found : {typeof(T)}");
            return false;
        }
    }
    public T? GetStat<T>(string key)
    {
        if (statParseStrategies.ContainsKey(typeof(T)))
        {
            var strategy = statParseStrategies[typeof(T)];
            if (strategy is IStatParseStrategy<T>)
            {
                return ((IStatParseStrategy<T>)strategy).GetStat(stats, key);
            }
            else
            {
                Logger.LogError($"[EntityBase] Stat parse Strategy for {typeof(T)} not match for IStatParseStrategy<{typeof(T)}>");
                return default(T);
            }
        }
        else
        {
            Logger.LogError($"[EntityBase] Stat parse Strategy not found : {typeof(T)}");
            return default(T);
        }
    }

    public T? SetStat<T>(string key, T value)
    {
        if (statParseStrategies.ContainsKey(typeof(T)))
        {
            var strategy = statParseStrategies[typeof(T)];
            if (strategy is IStatParseStrategy<T>)
            {
                return ((IStatParseStrategy<T>)strategy).SetStat(stats, key, value);
            }
            else
            {
                Logger.LogError($"[EntityBase] Stat parse Strategy for {typeof(T)} not match for IStatParseStrategy<{typeof(T)}>");
                return default(T);
            }
        }
        else
        {
            Logger.LogError($"[EntityBase] Stat parse Strategy not found : {typeof(T)}");
            return default(T);
        }
    }
}