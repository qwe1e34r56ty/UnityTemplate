using System.IO;
using System.Resources;
using UnityEngine;

public class EntitySpawner
{
    ResourceManager resourceManager;
    public EntitySpawner(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }
    public GameObject Spawn(EntityData elementData)
    {
        GameObject prefab = Resources.Load<GameObject>(elementData.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found:{elementData.prefabPath}");
            return null;
        }
        Texture2D texture = resourceManager.GetResource<Texture2D>(Path.Combine(Application.streamingAssetsPath, elementData.streamingAssetsSpritePath));
        if (texture == null)
        {
            Debug.LogError($"Texture File not found : {elementData.streamingAssetsSpritePath}");
            return null;
        }
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100f);
        GameObject gameObject = GameObject.Instantiate(prefab);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = elementData.sortingOrder;
        }
        gameObject.transform.position = new Vector3(elementData.x, elementData.y, 0f);
        gameObject.transform.localScale = new Vector3(elementData.width, elementData.height, 1f);
        gameObject.name = elementData.id;
        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject != null)
        {
            GameObject.Destroy(gameObject);
        }
    }
}