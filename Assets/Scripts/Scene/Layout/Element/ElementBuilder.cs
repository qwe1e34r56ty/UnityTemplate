using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;

public class ElementBuilder
{
    private ResourceManager resourceManager;
    public ElementBuilder(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }
    public GameObject ElementBuild(GameContext gameContext,
        string layoutID,
        ElementData elementData,
        Vector3? offsetPosition = null,
        Vector3? offsetRotation = null,
        Vector3? offsetScale = null,
        int? offsetSortingOrder = null)
    {
        if(!gameContext.layouts.TryGetValue(layoutID, out var layout))
        {
            return null;
        }
        if (layout.ContainsKey(elementData.id))
        {
            return null;
        }
        GameObject gameObject = new GameObject(elementData.id);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        if(!gameContext.animationMap.TryGetValue(elementData.animationID, out var animation))
        {
            Logger.LogWarning($"[ElementBuilder] Animation not found in animationMap {elementData.animationID}");
            return null;
        }

        spriteRenderer.sortingOrder = elementData.sortingOrder + (offsetSortingOrder ?? 0);
        gameObject.transform.position = new Vector3(elementData.x, elementData.y, 0f) + (offsetPosition ?? Vector3.zero);
        gameObject.transform.localScale = offsetScale ?? Vector3.one;
        gameObject.transform.rotation = offsetRotation.HasValue ? Quaternion.Euler(offsetRotation.Value) : Quaternion.identity;

        AnimationPlayer animationPlayer = new();
        gameContext.animationPlayerMap.Add(gameObject, animationPlayer);
        animationPlayer.Play(gameObject, animation);
        gameContext.updateHandlers.Add(animationPlayer);

        if (TagUtility.IsValidTagName(elementData.tagName))
        {
            gameObject.tag = elementData.tagName;
        }
        if (LayerUtility.IsValidLayerName(elementData.layerName))
        {
            gameObject.layer = LayerMask.NameToLayer(elementData.layerName);
        }
        spriteRenderer.sortingOrder += offsetSortingOrder ?? 0;

        layout.Add(elementData.id, gameObject);
        return gameObject;
    }

    public void ElementDestroy(GameContext gameContext, string layoutID, string elementID)
    {
        if (gameContext.layouts.TryGetValue(layoutID, out var layout))
        {
            if (layout.TryGetValue(elementID, out var element)) 
            {
                layout.Remove(elementID);
                if (gameContext.animationPlayerMap.TryGetValue(element, out var animationPlayer))
                {
                    gameContext.updateHandlers.Remove(animationPlayer);
                    gameContext.animationPlayerMap.Remove(element);
                }
                GameObject.Destroy(element);
            }
        }
    }
}