using System.Collections.Generic;
#nullable enable

public class IntParseStrategy : IStatParseStrategy<int>
{
    public bool TryGetStat(Dictionary<string, string> stats, string key, out int ret)
    {
        ret = default;
        if (stats.TryGetValue(key, out var str))
        {
            return int.TryParse(str, out ret);
        }
        return false;
    }

    public int GetStat(Dictionary<string, string> stats, string key)
    {
        if (TryGetStat(stats, key, out var result))
        {
            return result;
        }
        return default;
    }

    public int SetStat(Dictionary<string, string> stats, string key, int value)
    {
        stats[key] = value.ToString();
        return value;
    }
}