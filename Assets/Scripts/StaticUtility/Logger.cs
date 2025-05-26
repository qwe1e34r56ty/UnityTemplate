using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class Logger
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(string log)
    {
       Debug.Log(log);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string log)
    {
        Debug.LogWarning(log);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(string log)
    {
        Debug.LogError(log);
    }
}
