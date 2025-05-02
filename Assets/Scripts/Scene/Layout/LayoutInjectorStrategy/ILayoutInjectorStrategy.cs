using System.Collections.Generic;
using UnityEngine;

public interface ILayoutInjectorStrategy
{
    public void Inject(Dictionary<string, GameObject> layout);
}