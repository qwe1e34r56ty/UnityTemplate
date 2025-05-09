using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public static void Log(string log)
    {
#if UNITY_EDITOR
       Debug.Log(log);
#endif
    }

    public static void LogWarning(string log)
    {
#if UNITY_EDITOR
        Debug.LogWarning(log);
#endif
    }

    public static void LogError(string log)
    {
#if UNITY_EDITOR
        Logger.LogError(log);
#endif
    }
}
