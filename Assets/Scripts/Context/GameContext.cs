#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
public class GameContext
{
    public SaveData? saveData = new();
    public AScene? currentScene = null;
    public Queue<ISceneCommand> sceneCommandQueue = new();
    public Dictionary<string, Dictionary<string, GameObject>> entityComponentMap = new();
    public Dictionary<string, GameObject> entityRootMap = new();
    public Dictionary<GameObject, AnimationPlayer> animationPlayerMap = new();
    public Dictionary<string, AScene> sceneMap = new();
    public Dictionary<string, IAction> actionMap = new();

    //Resource
    public HashSet<string> layerNameSet = new();
    public HashSet<string> tagNameSet = new();
    public Dictionary<string, (Sprite[], AnimationPath)> animationDataMap = new();
    public Dictionary<string, EntityData> entityDataMap = new();
    public Dictionary<string, SceneData> sceneDataMap = new();

    // Handler
    public HashSet<IUpdatable> updateHandlers = new();
    public Dictionary<GameObject, Action> onHoverEnterHandlers = new();
    public Dictionary<GameObject, Action> onHoverExitHandlers = new();
    public Dictionary<GameObject, Action> onLeftClickHandlers = new();
    public Dictionary<GameObject, Action> onRightClickHandlers = new();
    public Queue<GameObject> pendingHoverEnterEventQueue = new();
    public Queue<GameObject> pendingHoverExitEventQueue = new();
    public Queue<GameObject> pendingLeftClickEventQueue = new();
    public Queue<GameObject> pendingRightClickEventQueue = new();
    public Dictionary<GameObject, Dictionary<KeyCode, Action>> onKeyDownHandlers = new();
    public Dictionary<GameObject, Dictionary<KeyCode, Action>> onKeyHoldHandlers = new();
    public Dictionary<GameObject, Dictionary<KeyCode, Action>> onKeyUpHandlers = new();
    public Queue<(GameObject, KeyCode)> pendingKeyDownEventQueue = new();
    public Queue<(GameObject, KeyCode)> pendingKeyHoldEventQueue = new();
    public Queue<(GameObject, KeyCode)> pendingKeyUpEventQueue = new();

    public GameContext()
    {
    }

    public GameContext(SaveData? saveData)
    {
        this.saveData = saveData;
    }

    public void ClearEventQueue()
    {
        pendingHoverEnterEventQueue.Clear();
        pendingHoverExitEventQueue.Clear();
        pendingLeftClickEventQueue.Clear();
        pendingRightClickEventQueue.Clear();

        pendingKeyDownEventQueue.Clear();
        pendingKeyHoldEventQueue.Clear();
        pendingKeyUpEventQueue.Clear();
    }

    public void ClearHandlers()
    {
        onHoverEnterHandlers.Clear();
        onHoverExitHandlers.Clear();
        onLeftClickHandlers.Clear();
        onRightClickHandlers.Clear();
        onKeyDownHandlers.Clear();
        onKeyHoldHandlers.Clear();
        onKeyUpHandlers.Clear();
        updateHandlers.Clear();
    }

    public void ClearBeforeLoadScene()
    {
        ClearHandlers();
        ClearEventQueue();
        entityRootMap.Clear();
        entityComponentMap.Clear();
    }
}
