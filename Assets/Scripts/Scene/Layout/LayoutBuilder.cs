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

    public GameObject LayoutBuild(Dictionary<string, Dictionary<string, GameObject>> layouts,
        Dictionary<string, GameObject> layoutRootMap,
        Dictionary<string, LayoutData> layoutDataMap,
        Dictionary<string, Sprite> spriteMap,
        string layoutID, 
        Vector3? offsetPosition = null, 
        Vector3? offsetRotation = null, 
        Vector3? offsetScale = null,
        int? offsetSortingOrder = null)
    {
        if (layouts.ContainsKey(layoutID))
        {
            return null;
        }
        GameObject layoutRoot = new GameObject(layoutID);
        layoutRoot.transform.position += offsetPosition ?? Vector3.zero;
        layoutRoot.transform.localScale = offsetScale ?? Vector3.one;
        layoutRoot.transform.rotation = offsetRotation.HasValue ? Quaternion.Euler(offsetRotation.Value) : Quaternion.identity;

        Dictionary<string, GameObject> layout = new();
        foreach (ElementData elementData in layoutDataMap[layoutID].elementDataArr)
        {
            GameObject element = elementBuilder.ElementBuild(layout, spriteMap, elementData, offsetSortingOrder: offsetSortingOrder);
            if(element != null)
            {
                element.transform.SetParent(layoutRoot.transform, false);
            }
        }
        layouts[layoutID] = layout;
        layoutRootMap[layoutID] = layoutRoot;
        return layoutRoot;
    }

    public void LayoutDestroy(Dictionary<string, Dictionary<string, GameObject>> layouts,
        Dictionary<string, GameObject> layoutRootMap,
        string layoutID)
    { 
        if (layoutRootMap.TryGetValue(layoutID, out var layoutRoot))
        {
            GameObject.Destroy(layoutRoot);
            layoutRootMap.Remove(layoutID);
        }
        if (layouts.ContainsKey(layoutID))
        {
            layouts.Remove(layoutID);
        }
    }
}
