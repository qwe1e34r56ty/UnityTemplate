using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


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
        if (instance == null)
        {
            saveManager = new();
            saveData = saveManager.Load();

            resourceManager = new();
            sceneDirector = new(resourceManager);

            instance = this;
            gameContext = new GameContext(saveData); // 순수 컨테이너

            gameContextInitializer = new GameContextInitializer(resourceManager);
            gameContextInitializer.InitGameContext(gameContext);

            mouseInputDetector = new MouseInputDetector();
            mouseInputDispatcher = new MouseInputDispatcher();

            keyboardInputDetector = new KeyboardInputDetector();
            keyboardInputDispatcher = new KeyboardInputDispatcher();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(SceneID.Start, $"Convert to {SceneID.Start} Scene request"));
	}

    // Update is called once per frame
    void Update()
    {
        mouseInputDetector.Detect(gameContext);
        mouseInputDispatcher.Dispatch(gameContext);
        keyboardInputDetector.Detect(gameContext);
        keyboardInputDispatcher.Dispatch(gameContext);
        sceneDirector.ExecuteSceneCommand(gameContext);

        gameContext.ClearEventQueue();
    }
}
