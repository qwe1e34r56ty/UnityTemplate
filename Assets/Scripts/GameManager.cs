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
        saveManager = new();
        saveData = saveManager.Load();

        resourceManager = new();
        sceneDirector = new(resourceManager);

        instance = this;
        gameContext = new GameContext(saveData);

        gameContextInitializer = new GameContextInitializer(resourceManager);
        gameContextInitializer.InitGameContext(gameContext);

        mouseInputDetector = new();
        mouseInputDispatcher = new();

        keyboardInputDetector = new();
        keyboardInputDispatcher = new();

        tickDispatcher = new();

        DontDestroyOnLoad(gameObject);
        gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(SceneID.Start, $"Convert to {SceneID.Start} Scene request"));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Update()
    {
        mouseInputDetector.Detect(gameContext);
        mouseInputDispatcher.Dispatch(gameContext);
        keyboardInputDetector.Detect(gameContext);
        keyboardInputDispatcher.Dispatch(gameContext);
        sceneDirector.ExecuteSceneCommand(gameContext);
        tickDispatcher.Dispatch(gameContext);

        gameContext.ClearEventQueue();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene" && gameContext.currentScene != null)
        {
            gameContext.sceneCommandQueue.Enqueue(
                new ConvertSceneCommand(gameContext.currentScene.id, $"Returned to {gameContext.currentScene.id}"));
        }
    }
}
