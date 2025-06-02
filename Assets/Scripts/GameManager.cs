using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private ResourceManager resourceManager;

    private SaveData saveData;
    private SaveManager saveManager;

    private GameContext gameContext;
    private GameContextInitializer gameContextInitializer;

    private SceneDirector sceneDirector;

    private MouseInputDetector mouseInputDetector;
    private MouseInputDispatcher mouseInputDispatcher;

    private KeyboardInputDetector keyboardInputDetector;
    private KeyboardInputDispatcher keyboardInputDispatcher;

    private TickDispatcher tickDispatcher;

    private void Awake()
    {
        Application.targetFrameRate = 600; 
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        saveManager = new SaveManager();
        saveData = saveManager.Load();

        resourceManager = new ResourceManager();
        sceneDirector = new SceneDirector(resourceManager);

        instance = this;
        gameContext = new GameContext(saveData);

        gameContextInitializer = new GameContextInitializer(resourceManager);
        gameContextInitializer.InitGameContext(gameContext);

        mouseInputDetector = new MouseInputDetector();
        mouseInputDispatcher = new MouseInputDispatcher();

        keyboardInputDetector = new KeyboardInputDetector();
        keyboardInputDispatcher = new KeyboardInputDispatcher();

        tickDispatcher = new TickDispatcher();

        DontDestroyOnLoad(gameObject);
        gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(SceneID.Start, $"Convert to {SceneID.Start} Scene request"));
    }

    private void Update()
    {
        mouseInputDetector.Detect(gameContext);
        mouseInputDispatcher.Dispatch(gameContext);
        keyboardInputDetector.Detect(gameContext);
        keyboardInputDispatcher.Dispatch(gameContext);
        sceneDirector.ExecuteSceneCommand(gameContext);
        tickDispatcher.Dispatch(gameContext);

        gameContext.ClearEventQueue();
    }

}
