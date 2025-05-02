using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameContext gameContext;
    public SaveData saveData;
    public SaveManager saveManager;
    public ResourceManager resourceManager;
    public ElementBuilder elementFactory;
    public SceneDirector sceneBuilder;
    public Queue<ISceneCommand> sceneCommandQueue;
    public Dictionary<string, IScene> sceneMap;
    public Dictionary<string, string> sceneLayoutMap;

    private void Awake()
    {
        Application.targetFrameRate = 600; 
        if (instance == null)
        {
            saveManager = new();
            saveData = saveManager.Load();

            resourceManager = new();
            sceneBuilder = new(resourceManager);

            instance = this;
            gameContext = new GameContext(saveData);
            initGameContext(gameContext);

            sceneCommandQueue = gameContext.GetSceneCommandQueue();
            sceneCommandQueue.Enqueue(new BuildLayoutCommand("Main"));

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
        sceneBuilder.UpdateScene(gameContext);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneCommandQueue.Count > 0)
        {
            //sceneBuilder.BuildScene(gameContext);
        }
    }
    void SetScene()
    {

    }

    void initGameContext(GameContext gameContext)
    {
        gameContext.layoutPathMap = new();
        initLayoutPathMap(gameContext.layoutPathMap);
        gameContext.layoutDataMap = new();
        initLayoutDataMap(gameContext.layoutDataMap, gameContext.layoutPathMap);
    }

    void initLayoutPathMap(Dictionary<string, string> layoutPathMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.LayoutPath);
        LayoutPath[] layoutPathArr = resourceManager.GetResource<LayoutPath[]>(path);
        foreach(LayoutPath layoutPath in layoutPathArr)
        {
            layoutPathMap[layoutPath.id] = layoutPath.path;
        }
    }
    void initLayoutDataMap(Dictionary<string, LayoutData> layoutDataMap, Dictionary<string, string> layoutPathMap)
    {
        string path;
        LayoutData layoutData;
        foreach(var pair in layoutPathMap)
        {
            path = Path.Combine(Application.streamingAssetsPath, pair.Value);
            layoutData = resourceManager.GetResource<LayoutData>(path);
            layoutDataMap[pair.Key] = layoutData;
        }
    }
}
