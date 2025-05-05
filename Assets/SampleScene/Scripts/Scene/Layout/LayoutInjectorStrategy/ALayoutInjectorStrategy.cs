using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public abstract class ALayoutInjectorStrategy
{
    public abstract void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout);

    public void ElementInject(GameContext gameContext,
        GameObject element,
        Action LeftClickAction = null,
        Action RightClickAction = null, 
        Action HoverEnterAction = null,
        Action HoverExitAction = null)
    {
        if (LeftClickAction != null)
        {
            gameContext.onLeftClickHandlers[element] = LeftClickAction;
        }
        if (RightClickAction != null)
        {
            gameContext.onRightClickHandlers[element] = RightClickAction;
        }
        if (HoverEnterAction != null)
        {
            gameContext.onHoverEnterHandlers[element] = HoverEnterAction;
        }
        if (HoverExitAction != null)
        {
            gameContext.onHoverExitHandlers[element] = HoverExitAction;
        }
    }
}