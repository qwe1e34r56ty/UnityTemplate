using System.Collections.Generic;
#nullable enable

public class FloatParseStrategy : IStatParseStrategy<float>
{
    public bool TryGetStat(Dictionary<string, string> stats, string key, out float ret)
    {
        ret = default;
        if (stats.TryGetValue(key, out var str))
        {
            return float.TryParse(str, out ret);
        }
        return false;
    }

    public float GetStat(Dictionary<string, string> stats, string key)
    {
        if(TryGetStat(stats, key, out var result))
        {
            return result;
        }
        return default;
    }

    public float SetStat(Dictionary<string, string> stats, string key, float value)
    {
        stats[key] = value.ToString();
        return value;
    }
}