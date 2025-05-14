using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EntityData
{
    public string typeID;
    public ComponentData[] componentDataArr;
    public Stat[] statArr;
    public ActionData[] actionDataArr;
}