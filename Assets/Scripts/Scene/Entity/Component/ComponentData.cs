using UnityEngine;
using System;

[Serializable]
public class ComponentData
{
    public string id;
    public string animationID;
    public string layerName;
    public string tagName;
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 offsetScale;
    public int offsetSortingOrder;
    public int sortingOrder;
}
