
using System.Collections.Generic;

public class StringParseStrategy : IStatParseStrategy<string>
{
    public bool TryGetStat(Dictionary<string, object> dictionary, string key, out string ret)
    {
        ret = default;
        if (dictionary.TryGetValue(key, out var obj) && obj is string str)
        {
            ret = str;
            return true;
        }
        return false;
    }

    public string GetStat(Dictionary<string, object> stats, string key)
    {
        if (TryGetStat(stats, key, out var result))
        {
            return result;
        }
        return default;
    }

    public string SetStat(Dictionary<string, object> stats, string key, string value)
    {
        stats[key] = value;
        return value;
    }
}