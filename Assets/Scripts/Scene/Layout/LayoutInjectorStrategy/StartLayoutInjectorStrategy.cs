using System.Collections.Generic;
using UnityEngine;

public class StartLayoutInjectorStrategy : ILayoutInjectorStrategy
{
    public void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout)
    {
        if (layout.ContainsKey(StartElementID.Start))
        {
            GameObject start = layout[StartElementID.Start];
            SpriteRenderer spriteRenderer = start.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var polygonCollider = start.AddComponent<PolygonCollider2D>();
                polygonCollider.isTrigger = true;
                gameContext.onLeftClickHandlers[start] = () =>
                {
                    Debug.Log("정 상 화");
                };
                // 예시
                gameContext.onHoverEnterHandlers[start] = () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[StartLayoutSpriteID.StartHoverEnter];
                };
                gameContext.onHoverExitHandlers[start] = () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[StartLayoutSpriteID.Start];
                };
            }
            else
            {
                Debug.LogError($"SpriteRenderer not found : {start.name}");
            }
        }
    }
}