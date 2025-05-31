using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EntityData
{
    public string id;
    public ComponentData[] componentDataArr;
    public string layerName;
    public string tagName;
    public ActionEntry[] actionWithPriorityArr;
    public StatEntry[] statKeyWithValueArr;
}