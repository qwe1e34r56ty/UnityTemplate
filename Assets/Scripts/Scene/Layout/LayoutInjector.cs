using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class LayoutInjector
{
    private Dictionary<string, ALayoutInjectorStrategy> layoutInjectorStrategies = new();
    public LayoutInjector()
    {
        layoutInjectorStrategies[LayoutID.StartScene] = new StartSceneLayoutInjectorStrategy();
        layoutInjectorStrategies[LayoutID.MainScene] = new MainSceneLayoutInjectorStrategy();
        layoutInjectorStrategies[LayoutID.EndScene] = new EndSceneLayoutInjectorStrategy();
    }

    public void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout,
        string layoutID)
    {
        if (layoutInjectorStrategies.ContainsKey(layoutID))
        {
            var strategy = layoutInjectorStrategies[layoutID];
            strategy.Inject(gameContext, layout);
        }
        else
        {
            Debug.LogError($"Inject Strategy not found : {layoutID}");
        }
    }

    public void Eject(GameContext gameContext,
        Dictionary<string, GameObject> layout,
        string layoutID)
    {
        foreach(var pair in layout)
        {
            gameContext.onHoverEnterHandlers.Remove(pair.Value);
            gameContext.onHoverExitHandlers.Remove(pair.Value);
            gameContext.onLeftClickHandlers.Remove(pair.Value);

            gameContext.onKeyDownHandlers.Remove(pair.Value);
            gameContext.onKeyUpHandlers.Remove(pair.Value);
            gameContext.onKeyHoldHandlers.Remove(pair.Value);
        }
    }

}