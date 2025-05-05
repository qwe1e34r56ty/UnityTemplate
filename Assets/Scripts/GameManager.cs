using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ResourceManager resourceManager;

    public SaveData saveData;
    public SaveManager saveManager;

    public GameContext gameContext;
    public GameContextInitializer gameContextInitializer;

    public SceneDirector sceneDirector;

    public MouseInputDetector mouseInputDetector;
    public MouseInputDispatcher mouseInputDispatcher;

    public KeyboardInputDetector keyboardInputDetector;
    public KeyboardInputDispatcher keyboardInputDispatcher;

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

        mouseInputDetector = new MouseInputDetector();
        mouseInputDispatcher = new MouseInputDispatcher();

        keyboardInputDetector = new KeyboardInputDetector();
        keyboardInputDispatcher = new KeyboardInputDispatcher();

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
                new ConvertSceneCommand(gameContext.currentScene.sceneID, $"Returned to {gameContext.currentScene.sceneID}"));
        }
    }
}
