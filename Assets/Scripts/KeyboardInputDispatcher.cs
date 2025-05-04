using UnityEngine;

public class KeyboardInputDispatcher
{
    public void Dispatch(GameContext context)
    {
        GameObject gameObject;
        KeyCode keyCode;
        while (context.pendingKeyDownEventQueue.TryDequeue(out var gameObjectKey))
        {
            gameObject = gameObjectKey.Item1;
            keyCode = gameObjectKey.Item2;
            if (context.onKeyDownHandlers.ContainsKey(gameObject) &&
                context.onKeyDownHandlers[gameObject].ContainsKey(keyCode))
            {
                context.onKeyDownHandlers[gameObject][keyCode].Invoke();
            }
        }
        while (context.pendingKeyUpEventQueue.TryDequeue(out var gameObjectKey))
        {
            if (context.onKeyUpHandlers.ContainsKey(gameObjectKey.Item1) &&
                context.onKeyUpHandlers[gameObjectKey.Item1].ContainsKey(gameObjectKey.Item2))
            {
                context.onKeyUpHandlers[gameObjectKey.Item1][gameObjectKey.Item2].Invoke();
            }
        }
        while (context.pendingKeyHoldEventQueue.TryDequeue(out var gameObjectKey))
        {
            if (context.onKeyHoldHandlers.ContainsKey(gameObjectKey.Item1) &&
                context.onKeyHoldHandlers[gameObjectKey.Item1].ContainsKey(gameObjectKey.Item2))
            {
                context.onKeyHoldHandlers[gameObjectKey.Item1][gameObjectKey.Item2].Invoke();
            }
        }
    }
}