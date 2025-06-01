using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EntityData
{
    public string id;
    public EntityData[] entityDataArr;
    public string layerName;
    public string tagName;
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 offsetScale;
    public ActionEntry[] actionWithPriorityArr;
    public StatEntry[] statKeyWithValueArr;
}

[Serializable]
public class ActionEntry
{
    public string id;
    public int priority;
}

[Serializable]
public class StatEntry
{
    public string key;
    public string value;
}