using System.Collections.Generic;
using System.IO;
using System.Resources;
using UnityEngine;

public class ElementBuilder
{
    ResourceManager resourceManager;
    public ElementBuilder(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }
    public void Build(Dictionary<string, GameObject> layout, ElementData elementData)
    {
        if (layout.ContainsKey(elementData.id))
        {
            return;
        }
        GameObject gameObject = new GameObject(elementData.id);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        Texture2D texture = resourceManager.GetResource<Texture2D>(Path.Combine(Application.streamingAssetsPath, elementData.streamingAssetsSpritePath));
        if (texture == null)
        {
            Debug.LogError($"Texture File not found : {elementData.streamingAssetsSpritePath}");
        }
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, elementData.pixelPerUnit);
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = elementData.sortingOrder;
        }
        gameObject.transform.position = new Vector3(elementData.x, elementData.y, 0f);
        gameObject.transform.localScale = new Vector3(elementData.width, elementData.height, 1f);
        layout.Add(elementData.id, gameObject);
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject != null)
        {
            GameObject.Destroy(gameObject);
        }
    }
}