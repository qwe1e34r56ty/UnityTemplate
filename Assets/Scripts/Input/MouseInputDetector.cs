using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseInputDetector
{
    private GameObject lastHovered = null;

    public void Detect(GameContext context)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        if (hits.Length == 0)
        {
            if (lastHovered != null)
            {
                context.pendingHoverExitEventQueue.Enqueue(lastHovered);
                lastHovered = null;
            }
            return;
        }

        RaycastHit2D topHit = hits.OrderByDescending(hit => hit.collider.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0).First();

        GameObject current = topHit.collider.gameObject;
        if (current != lastHovered)
        {
            if (lastHovered != null)
            {
                context.pendingHoverExitEventQueue.Enqueue(lastHovered);
            }
            if (current != null)
            {
                context.pendingHoverEnterEventQueue.Enqueue(current);
            }
            lastHovered = current;
        }

        if (current != null &&
            Input.GetMouseButtonDown(0))
        {
            context.pendingLeftClickEventQueue.Enqueue(current);
        }
        if (current != null &&
            Input.GetMouseButtonDown(1))
        {
            context.pendingRightClickEventQueue.Enqueue(current);
        }
    }
}
