#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using System.Linq;

public static class TagUtility
{

    public static bool IsValidTagName(string tagName)
{
#if UNITY_EDITOR
    if (!InternalEditorUtility.tags.Contains(tagName))    {
        UnityEngine.Debug.LogWarning($"[TagUtility] {tagName} Tag not found.");
        return false;
    }
#endif
        return true;
}
}