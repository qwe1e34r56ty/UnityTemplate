using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine;

public static class LayerUtility
{
    public static bool IsValidLayerName(string layerName)
    {
        if (LayerMask.NameToLayer(layerName) == -1)
        {
            Logger.LogError($"[LayerUtility] {layerName} Layer not found, please add Layer ");
            return false;
        }
        return true;
    }
}