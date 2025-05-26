using System.Collections.Generic;
using UnityEngine;

public class Entity : EntityBase
{
    public readonly string typeID;
    public Dictionary<string, GameObject> components;
    private SortedList<int, List<IAction>> sortedActionList;
    public Entity(GameContext gameContext, EntityData entityData, Vector3 offsetPosition, Vector3 offsetRotationEuler, Vector3 offsetScale, int offsetSortOrder) : base(entityData)
    {
       
    }
}