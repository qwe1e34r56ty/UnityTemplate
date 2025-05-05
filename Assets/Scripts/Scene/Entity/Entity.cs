using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

public class Entity
{
    public string typeID = "";
    public string name = "";
    public GameObject gameObject { get; }
    public Dictionary<string, IAttribute> attributes = new();
    public List<AEntityAction> actions = new();

    public Entity(string typeID, string name, GameObject gameObject)
    {
        this.typeID = typeID;
        this.name = name;
        this.gameObject = gameObject;
    }

    public void Act(GameContext context)
    {
        foreach (var action in actions.OrderByDescending(a => a.priority))
        {
            if (action.CanExecute(this, context))
            {
                action.Execute(this, context);
                break;
            }
        }
    }
}