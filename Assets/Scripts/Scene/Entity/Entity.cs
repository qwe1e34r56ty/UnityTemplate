using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : EntityBase, IUpdatable
{
    public GameObject root { get; private set; }
    private Dictionary<string, GameObject> components;
    private Dictionary<GameObject, AnimationPlayer> animationPlayers;
    private SortedList<int, List<IAction>> sortedActionList;
    public Entity(GameContext gameContext, 
        ResourceManager resourceManager,
        EntityData entityData, 
        Vector3? offsetPosition = null, 
        Vector3? offsetRotation = null, 
        Vector3? offsetScale = null, 
        int offsetSortOrder = 0) : base(entityData)
    {
        root = new(entityData.id);
        components = new();
        animationPlayers = new();
        sortedActionList = new();

        Transform rootTransform = root.transform;
        rootTransform.position += offsetPosition ?? Vector3.zero;
        rootTransform.rotation *= Quaternion.Euler(offsetRotation ?? Vector3.zero);
        rootTransform.localScale = Vector3.Scale(rootTransform.localScale, offsetScale ?? Vector3.one);

        foreach (ComponentData componentData in entityData.componentDataArr)
        {
            GameObject component = new(componentData.id);

            Transform componentTransform = component.transform;
            componentTransform.position += componentData.offsetPosition;
            componentTransform.rotation *= Quaternion.Euler(componentData.offsetRotation);
            componentTransform.localScale = Vector3.Scale(componentTransform.localScale, componentData.offsetScale);
            componentTransform.SetParent(root.transform, false);

            if (gameContext.animationDataMap.TryGetValue(componentData.animationID, out (Sprite[], AnimationPath) animationData)) {
                SpriteRenderer spriteRenderer = component.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder += componentData.offsetSortingOrder;

                AnimationPlayer animationPlayer = new();
                animationPlayer.Play(component, animationData);
                gameContext.updateHandlers.Add(animationPlayer);

                gameContext.animationPlayerMap.Add(component, animationPlayer);
                animationPlayers.Add(component, animationPlayer);
            }
            components.Add(componentData.id, component);
            gameContext.entities.Add(component, this);
        }
    }

    public void Update(float deltaTime)
    {

    }

    public void Destroy(GameContext gameContext)
    {
        gameContext.entities.Remove(root);
        foreach (GameObject gameObject in components.Values)
        {
            if (animationPlayers.ContainsKey(gameObject))
            {
                {
                    animationPlayers.Remove(gameObject);
                }
                if (gameContext.animationPlayerMap.TryGetValue(gameObject, out AnimationPlayer animationPlayer))
                {
                    gameContext.updateHandlers.Remove(animationPlayer);
                }
                gameContext.animationPlayerMap.Remove(gameObject);
                gameContext.entities.Remove(gameObject);
            }
            GameObject.Destroy(root);
        }
    }
}
