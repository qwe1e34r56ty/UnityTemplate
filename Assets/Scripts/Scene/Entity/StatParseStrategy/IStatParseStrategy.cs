using System.Collections.Generic;
#nullable enable

public interface IStatParseStrategy<T>
{
    public bool TryGetStat(Dictionary<string, object> stats, string key, out T? value);
    public T? GetStat(Dictionary<string, object> stats, string key);
    public T? SetStat(Dictionary<string, object> stats, string key, T value);
}