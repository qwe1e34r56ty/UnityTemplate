using System.Collections.Generic;
using System.IO;
using System.Resources;
using UnityEngine;

public class EntitySpawner
{
    private ResourceManager resourceManager;
    public EntitySpawner(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }
    public GameObject EntitySpawn(Dictionary<string, GameObject> layout,
        Dictionary<string, Sprite> spriteMap,
        ElementData elementData,
        Vector3? offsetPosition = null,
        Vector3? offsetRotation = null,
        Vector3? offsetScale = null,
        int? offsetSortingOrder = null)
    {
        if (layout.ContainsKey(elementData.id))
        {
            return null;
        }
        GameObject gameObject = new GameObject(elementData.id);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        if (!spriteMap.TryGetValue(elementData.spriteID, out var sprite))
        {
            Debug.LogWarning($"Sprite not found in spriteMap {elementData.spriteID}");
            return null;
        }
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = elementData.sortingOrder;
        gameObject.transform.position = new Vector3(elementData.x, elementData.y, 0f) + (offsetPosition ?? Vector3.zero);
        gameObject.transform.localScale = Vector3.Scale(new Vector3(elementData.width, elementData.height, 1f), offsetScale ?? Vector3.one);
        gameObject.transform.rotation = offsetRotation.HasValue ? Quaternion.Euler(offsetRotation.Value) : Quaternion.identity;
        spriteRenderer.sortingOrder += offsetSortingOrder ?? 0;

        layout.Add(elementData.id, gameObject);
        return gameObject;
    }

    public void EntityDestroy(GameObject gameObject)
    {
        if (gameObject != null)
        {
            GameObject.Destroy(gameObject);
        }
    }
}