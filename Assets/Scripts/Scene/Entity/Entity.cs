using System.Collections.Generic;
using UnityEngine;

public class Entity : EntityBase
{
    public readonly string typeID;
    public GameObject root;
    public Dictionary<string, GameObject> components;
    private SortedList<int, List<IAction>> sortedActionList;
    public Entity(GameContext gameContext, EntityData entityData, ) : base(entityData)
    {
       
    }
}