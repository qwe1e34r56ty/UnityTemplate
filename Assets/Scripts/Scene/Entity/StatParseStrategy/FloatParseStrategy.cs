using System.Collections.Generic;
#nullable enable

public class FloatParseStrategy : IStatParseStrategy<float>
{
    public bool TryGetStat(Dictionary<string, object> dictionary, string key, out float ret)
    {
        ret = default;
        if (dictionary.TryGetValue(key, out var obj) && obj is string str)
        {
            return float.TryParse(str, out ret);
        }
        return false;
    }

    public float GetStat(Dictionary<string, object> stats, string key)
    {
        if(TryGetStat(stats, key, out var result))
        {
            return result;
        }
        return default;
    }

    public float SetStat(Dictionary<string, object> stats, string key, float value)
    {
        stats[key] = value.ToString();
        return value;
    }
}