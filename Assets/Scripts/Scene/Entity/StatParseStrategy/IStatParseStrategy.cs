using System.Collections.Generic;
#nullable enable

public interface IStatParseStrategy<T>
{
    public bool TryGetStat(Dictionary<string, string> stats, string key, out T? value);
    public T? GetStat(Dictionary<string, string> stats, string key);
    public T? SetStat(Dictionary<string, string> stats, string key, T value);
}