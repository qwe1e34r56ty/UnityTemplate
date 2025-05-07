using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PunchGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject sprite;
    private Sprite[] idle;
    private Sprite[] punch;
    private Sprite[] uppercut;
    private AnimationPlayer player = new();
    private static PunchGameManager instance;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int highScore = 0;
    int score = 0;

    private bool isPlaying = false;
    private bool isReady = true;
    private bool isThisScene = false;

    public static PunchGameManager getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        punch = Resources.LoadAll<Sprite>("Animations/Punch/Player/Punch");
        uppercut = Resources.LoadAll<Sprite>("Animations/Punch/Player/Uppercut");
    }

    void Start()
    {
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PunchGameScene")
        {
            isThisScene = true;
            isPlaying = false;
            isReady = true;
            LoadHighScore();
            UpdateScore();
            StartCoroutine(ReadyToStart());
        }
        else
        {
            isThisScene = false;
        }
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("PunchHighScore", 0);
    }

    void SaveHighScore(int newHighScore)
    {
        if (highScore < newHighScore)
        {
            PlayerPrefs.SetInt("PunchHighScore", newHighScore);
            highScore = newHighScore;
        }
    }

    private IEnumerator ReadyToStart()
    {
        isReady = true;
        for (int i = 0; i < 5; i++)
        {
            readyText.SetText($"{5 - i}!");
            yield return new WaitForSeconds(1f);
        }
        isPlaying = true;
        readyText.SetText("");
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isThisScene != true)
        {
            return;
        }
        if(isReady != true)
        {
            readyText.SetText("Exit : E\n Restart : R");
            if (Input.GetKeyDown(KeyCode.E))
            {
                isThisScene = false;
                SceneManager.LoadScene("SampleScene");
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
            return;
        }
        if(isPlaying != true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            player.Play(sprite, punch, 0.1f, false);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            player.Play(sprite, uppercut, 0.1f, false);
        }
        UpdateScore();
        player.Update(Time.deltaTime);
        RefreshCollider();
    }

    void UpdateScore()
    {
        scoreText.SetText($"Now Score : {score}\nHigh Score : {highScore}");
    }

    void RefreshCollider()
    {
        PolygonCollider2D oldCollider = sprite.GetComponent<PolygonCollider2D>();
        if (oldCollider)
        {
            Destroy(oldCollider);
        }
        sprite.AddComponent<PolygonCollider2D>();
    }


    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void Restart()
    {
        StartCoroutine(ReadyToStart());
    }

    public void GameOver()
    {
        isPlaying = false;
        SaveHighScore(score);
        UpdateScore();
        isReady = false;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }
}
