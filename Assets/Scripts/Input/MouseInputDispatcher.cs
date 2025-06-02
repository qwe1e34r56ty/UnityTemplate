using UnityEngine;

public class MouseInputDispatcher
{
    public void Dispatch(GameContext context)
    {
        while (context.pendingHoverEnterEventQueue.TryDequeue(out GameObject gameObject))
        {
            if (context.onHoverEnterHandlers.ContainsKey(gameObject))
            {
                context.onHoverEnterHandlers[gameObject].Invoke();
            }
        }
        while (context.pendingHoverExitEventQueue.TryDequeue(out GameObject gameObject))
        {
            if (context.onHoverExitHandlers.ContainsKey(gameObject))
            {
                context.onHoverExitHandlers[gameObject].Invoke();
            }
        }
        while (context.pendingLeftClickEventQueue.TryDequeue(out GameObject gameObject))
        {
            if (context.onLeftClickHandlers.ContainsKey(gameObject))
            {
                context.onLeftClickHandlers[gameObject].Invoke();
            }
        }
        while (context.pendingRightClickEventQueue.TryDequeue(out GameObject gameObject))
        {
            if (context.onRightClickHandlers.ContainsKey(gameObject))
            {
                context.onRightClickHandlers[gameObject].Invoke();
            }
        }
    }
}