using System.Collections.Generic;
using System.IO;
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

    public void BuildLayout(Dictionary<string, Dictionary<string, GameObject>> layouts,
        Dictionary<string, LayoutData> layoutDataMap,
        string layoutID)
    {
        if (layouts.ContainsKey(layoutID))
        {
            return;
        }
        Dictionary<string, GameObject> layout = new();
        foreach (ElementData elementData in layoutDataMap[layoutID].elementDataArr)
        {
            elementBuilder.Build(layout, elementData);
        }
        layouts.Add(layoutID, layout);
    }

    public void DestroyLayout(Dictionary<string, Dictionary<string, GameObject>> layouts,
        string layoutID)
    {
        foreach(var obj in layouts[layoutID])
        {
            if (obj.Value != null)
            {
                elementBuilder.Destroy(obj.Value);
            }
        }
        layouts.Remove(layoutID);
    }
}
