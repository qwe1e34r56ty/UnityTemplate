using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class LayoutBuilder
{
    private ElementBuilder elementBuilder;
    private ResourceManager resourceManager;
    public LayoutBuilder(ResourceManager resourceManager)
    {
        this.elementBuilder = new(resourceManager);
        this.resourceManager = resourceManager;
    }

    public GameObject LayoutBuild(GameContext gameContext,
        string layoutID, 
        Vector3? offsetPosition = null, 
        Vector3? offsetRotation = null, 
        Vector3? offsetScale = null,
        int? offsetSortingOrder = null)
    {
        if (gameContext.layouts.ContainsKey(layoutID))
        {
            return null;
        }
        GameObject layoutRoot = new GameObject(layoutID);
        layoutRoot.transform.position += offsetPosition ?? Vector3.zero;
        layoutRoot.transform.localScale = offsetScale ?? Vector3.one;
        layoutRoot.transform.rotation = offsetRotation.HasValue ? Quaternion.Euler(offsetRotation.Value) : Quaternion.identity;

        Dictionary<string, GameObject> layout = new();
        foreach (ElementData elementData in gameContext.layoutDataMap[layoutID].elementDataArr)
        {
            GameObject element = elementBuilder.ElementBuild(gameContext, layoutID, elementData, offsetSortingOrder: offsetSortingOrder);
            if(element != null)
            {
                element.transform.SetParent(layoutRoot.transform, false);
            }
        }
        gameContext.layouts[layoutID] = layout;
        gameContext.layoutRootMap[layoutID] = layoutRoot;
        return layoutRoot;
    }

    public void LayoutDestroy(GameContext gameContext,
        string layoutID)
    { 

        if (gameContext.layoutRootMap.TryGetValue(layoutID, out var layoutRoot))
        {
            GameObject.Destroy(layoutRoot);
            gameContext.layoutRootMap.Remove(layoutID);
        }
        if (gameContext.layouts.ContainsKey(layoutID))
        {
            gameContext.layouts.Remove(layoutID);
        }
    }
}
