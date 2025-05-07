using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI restartText;
    public TextMeshProUGUI highScoreText;
    int highScore = 0;

    public void Start()
    {
        if (restartText == null)
        {
            Debug.LogError("restart text is null");
        }

        if (scoreText == null)
        {
            Debug.LogError("scoreText is null");
            return;
        }

        restartText.gameObject.SetActive(false);
    }

    public void SetRestart()
    {
        restartText.gameObject.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        LoadHighScore();
        scoreText.text = $"NowScore : {score.ToString()}";
        highScoreText.text = $"HighScore : {highScore}";
    }

    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("PlaneHighScore", 0);
    }
}