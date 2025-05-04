using System.Collections.Generic;
using UnityEngine;

public interface ILayoutInjectorStrategy
{
    public void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout);
}