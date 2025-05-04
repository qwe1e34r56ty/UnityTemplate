using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputDetector
{
    public void Detect(GameContext context)
    {
        foreach (var pair in context.onKeyDownHandlers)
        {
            foreach (var keyAction in pair.Value)
            {
                if (Input.GetKeyDown(keyAction.Key))
                {
                    context.pendingKeyDownEventQueue.Enqueue((pair.Key, keyAction.Key));
                }
            }
        }

        foreach (var pair in context.onKeyHoldHandlers)
        {
            foreach (var keyAction in pair.Value)
            {
                if (Input.GetKey(keyAction.Key))
                {
                    context.pendingKeyHoldEventQueue.Enqueue((pair.Key, keyAction.Key));
                }
            }
        }

        foreach (var pair in context.onKeyUpHandlers)
        {
            foreach (var keyAction in pair.Value)
            {
                if (Input.GetKeyUp(keyAction.Key))
                {
                    context.pendingKeyUpEventQueue.Enqueue((pair.Key, keyAction.Key));
                }
            }
        }
    }
}