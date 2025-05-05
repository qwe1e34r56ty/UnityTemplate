using UnityEngine;
using System;

[Serializable]
public class EntityData
{
    public string typeId;
    public string spriteID;
    public float width;
    public float height;
    public int sortingOrder;
    public string[] attributeIDArr;
    public string[] actionIDArr;
}
