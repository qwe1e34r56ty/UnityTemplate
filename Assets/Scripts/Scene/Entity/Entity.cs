using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : EntityBase, IUpdateable
{
    public GameObject root { get; private set; }
    private AnimationPlayer animationPlayer;
    private HashSet<GameObject> childs;
    private SortedList<int, List<IAction>> sortedActionList;
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
        animationPlayer = null;
        childs = new();
        sortedActionList = new();
        if(entityRoot == null)
        {
            entityRoot = this;
        }

        root = new(entityData.id);
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

            spriteRenderer.sortingOrder += offsetSortOrder + entityData.offsetSortingOrder;
        }

        if (gameContext.animationDataMap.TryGetValue(entityData.animationID, out (Sprite[], AnimationPath)animationData))
        {
            animationPlayer = new AnimationPlayer();
            animationPlayer.Play(root, animationData);
            gameContext.updateHandlers.Add(animationPlayer);
            gameContext.animationPlayerMap.Add(root, animationPlayer);
        }

        foreach (EntityData _entityData in entityData.entityDataArr)
        {
            Entity entity = new Entity(gameContext, resourceManager, _entityData, entityRoot);
            childs.Add(entity.root);
        }

        foreach (var entry in entityData.actionWithPriorityArr)
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

        foreach (var pair in sortedActionList.ToList())
        {
            foreach (var action in pair.Value.ToList())
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

        if (gameContext.animationPlayerMap.TryGetValue(root, out AnimationPlayer animationPlayer))
        {
            gameContext.updateHandlers.Remove(animationPlayer);
            gameContext.animationPlayerMap.Remove(root);
        }

        GameObject.Destroy(root);
    }

    public void Update(GameContext gameContext, float deltaTime)
    {
        foreach (var pair in sortedActionList)
        {
            foreach (var action in pair.Value)
            {
                if (action.CanExecute(gameContext, this, deltaTime))
                {
                    action.Execute(gameContext, this, deltaTime);
                }
            }
        }
    }

    public void AttachAction(GameContext gameContext, IAction action, int priority)
    {
        if (!sortedActionList.TryGetValue(priority, out var list))
        {
            list = new List<IAction>();
            sortedActionList[priority] = list;
        }
        action.Attach(gameContext, this, priority);
        list.Add(action);
    }

    public void DetachAction(GameContext gameContext, IAction actionToRemove)
    {
        var oldActionList = new List<(int, List<IAction>)>();
        foreach (var pair in sortedActionList)
        {
            oldActionList.Add((pair.Key, pair.Value));
        }

        foreach (var pair in oldActionList)
        {
            foreach (var action in pair.Item2)
            {
                action.Detach(gameContext, this);
            }
        }

        ClearAction();

        foreach (var pair in oldActionList)
        {
            int priority = pair.Item1;
            foreach (var action in pair.Item2)
            {
                if (action == actionToRemove)
                    continue;

                AttachAction(gameContext, action, priority);
            }
        }
    }

    public void ClearAction()
    {
        sortedActionList.Clear();
    }
}
