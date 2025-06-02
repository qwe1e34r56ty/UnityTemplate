using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : EntityBase, IUpdateable
{
    public GameObject root { get; private set; }
    private readonly HashSet<GameObject> childs;
    private readonly SortedList<int, List<IAction>> sortedActionList;
    public Entity(GameContext gameContext,
        ResourceManager resourceManager,
        EntityData entityData, 
        Entity? entityRoot = null,
        Vector3? offsetPosition = null, 
        Vector3? offsetRotation = null, 
        Vector3? offsetScale = null, 
        int offsetSortOrder = 0) : base(entityData)
    {
        root = null;
        childs = new HashSet<GameObject>();
        sortedActionList = new SortedList<int, List<IAction>>();
        entityRoot = entityRoot ?? this;

        root = new GameObject(entityData.id);
        {
            SpriteRenderer spriteRenderer = root.AddComponent<SpriteRenderer>();
            root.layer = LayerMask.NameToLayer(entityData.layerName);
            root.tag = entityData.tagName;

            Transform rootTransform = root.transform;

            rootTransform.position += offsetPosition ?? Vector3.zero;
            rootTransform.position += entityData.offsetPosition;

            rootTransform.rotation *= Quaternion.Euler(offsetRotation ?? Vector3.zero);
            rootTransform.rotation = Quaternion.Euler(entityData.offsetRotation);

            rootTransform.localScale = Vector3.Scale(rootTransform.localScale, offsetScale ?? Vector3.one);
            rootTransform.localScale = Vector3.Scale(rootTransform.localScale, entityData.offsetScale);
        }

        foreach (EntityData _entityData in entityData.entityDataArr)
        {
            Entity entity = new Entity(gameContext, resourceManager, _entityData, entityRoot);
            childs.Add(entity.root);
        }

        foreach (ActionEntry entry in entityData.actionWithPriorityArr)
        {
            int priority = entry.priority;
            if (gameContext.actionMap.TryGetValue(entry.id, out var action))
            {
                AttachAction(gameContext, action, priority);
            }
        }

        gameContext.entities.Add(root, this);
        gameContext.entityRoots.Add(root, entityRoot);
    }

    public void Destroy(GameContext gameContext)
    {
        gameContext.entityRoots.Remove(root);
        gameContext.entities.Remove(root);

        foreach (KeyValuePair<int, List<IAction>> pair in sortedActionList.ToList())
        {
            foreach (IAction action in pair.Value.ToList())
            {
                DetachAction(gameContext, action);
            }
        }
        sortedActionList.Clear();

        foreach (GameObject gameObject in childs)
        {
            if (gameContext.entities.TryGetValue(gameObject, out Entity entity))
            {
                entity.Destroy(gameContext);
            }
        }
        GameObject.Destroy(root);
    }

    public void Update(GameContext gameContext, float deltaTime)
    {
        foreach (KeyValuePair<int, List<IAction>> pair in sortedActionList)
        {
            foreach (IAction action in pair.Value)
            {
                if (action.CanExecute(gameContext, this, deltaTime))
                {
                    action.Execute(gameContext, this, deltaTime);
                }
            }
        }
    }

    private void AttachAction(GameContext gameContext, IAction action, int priority)
    {
        if (!sortedActionList.TryGetValue(priority, out var list))
        {
            list = new List<IAction>();
            sortedActionList[priority] = list;
        }
        action.Attach(gameContext, this, priority);
        list.Add(action);
    }

    private void DetachAction(GameContext gameContext, IAction actionToRemove)
    {
        List<(int, List<IAction>)> oldActionList = new List<(int, List<IAction>)>();
        foreach (KeyValuePair<int, List<IAction>> pair in sortedActionList)
        {
            oldActionList.Add((pair.Key, pair.Value));
        }

        foreach ((int, List<IAction>) pair in oldActionList)
        {
            foreach (IAction action in pair.Item2)
            {
                action.Detach(gameContext, this);
            }
        }

        ClearAction();

        foreach ((int priority, List<IAction> actions) in oldActionList)
        {
            foreach (IAction action in actions)
            {
                if (action == actionToRemove)
                    continue;

                AttachAction(gameContext, action, priority);
            }
        }
    }

    private void ClearAction()
    {
        sortedActionList.Clear();
    }
}
