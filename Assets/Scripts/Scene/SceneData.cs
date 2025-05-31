using System;
using UnityEngine;

[Serializable]
public class SceneData
{
    public string id;
    public EntityTransformData[] entityTransformDataArr;
}

[Serializable]
public class EntityTransformData
{
    public string id;
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 offsetScale;
    public int offsetSortingOrder;
}