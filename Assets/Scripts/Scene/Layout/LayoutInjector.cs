using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class LayoutInjector
{
    public Dictionary<string, ILayoutInjectorStrategy> layoutInjectorStrategies;
    public LayoutInjector()
    {
        layoutInjectorStrategies[LayoutID.Main] = new MainLayoutInjectorStrategy();
    }

    public void Inject(Dictionary<string, GameObject> layout, string layoutID)
    {
        if (layoutInjectorStrategies.ContainsKey(layoutID))
        {
            var strategy = layoutInjectorStrategies[layoutID];
            strategy.Inject(layout);
        }
        else
        {
            Debug.LogError($"Strategy not found : {layoutID}");
        }
    }
    
}