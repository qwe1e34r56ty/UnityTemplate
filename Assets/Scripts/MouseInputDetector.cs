using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputDetector
{
    private GameObject lastHovered = null;

    public void Detect(GameContext context)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        GameObject current = hit.collider ? hit.collider.gameObject : null;
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
            Debug.Log("left Click");
        }
    }
}
