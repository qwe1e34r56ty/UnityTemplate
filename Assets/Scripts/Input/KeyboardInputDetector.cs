using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputDetector
{
    public void Detect(GameContext context)
    {
        foreach (var pair in context.onKeyDownHandlers)
        {
            foreach (KeyCode key in pair.Value.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    context.pendingKeyDownEventQueue.Enqueue((pair.Key, key));
                }
            }
        }

        foreach (var pair in context.onKeyHoldHandlers)
        {
            foreach (KeyCode key in pair.Value.Keys)
            {
                if (Input.GetKey(key))
                {
                    context.pendingKeyHoldEventQueue.Enqueue((pair.Key, key));
                }
            }
        }

        foreach (var pair in context.onKeyUpHandlers)
        {
            foreach (KeyCode key in pair.Value.Keys)
            {
                if (Input.GetKeyUp(key))
                {
                    context.pendingKeyUpEventQueue.Enqueue((pair.Key, key));
                }
            }
        }
    }
}