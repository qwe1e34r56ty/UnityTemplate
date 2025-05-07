using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneGameManager : MonoBehaviour
{
    static PlaneGameManager planeGameManager;

    public static PlaneGameManager Instance
    {
        get { return planeGameManager; }
    }

    private int currentScore = 0;
    UIManager uiManager;

    public UIManager UIManager
    {
        get { return uiManager; }
    }
    private void Awake()
    {
        planeGameManager = this;
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        uiManager.UpdateScore(0);
    }

    public void GameOver()
    {
        SaveHighScore(currentScore);
        Debug.Log("Game Over");
        uiManager.SetRestart();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        uiManager.UpdateScore(currentScore);
        Debug.Log("Score: " + currentScore);
    }

    public void SaveHighScore(int score)
    {
        int highScore = PlayerPrefs.GetInt("PlaneHighScore", 0);
        if (highScore < score)
        {
            PlayerPrefs.SetInt("PlaneHighScore", score);
        }
    }

}