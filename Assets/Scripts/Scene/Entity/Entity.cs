using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

public class Entity
{
    public string typeID = "";
    public string name = "";
    public GameObject[] components { get; }
    public HashSet<string> attributes = new();
    public Dictionary<string, AEntityAction> actions = new();

    public Entity(EntityData entityData, string name)
    {
        this.typeID = entityData.typeID;
        this.name = name;
    }

    public void Act(GameContext context)
    {

    }
}