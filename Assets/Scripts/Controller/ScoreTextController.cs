using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int punchHighScore = PlayerPrefs.GetInt("PunchHighScore", 0);
        int stackHighScore = PlayerPrefs.GetInt("PlaneHighScore", 0);

        scoreText.SetText($"Stack Best Score : {stackHighScore}\n" +
            $"Punch Best Score : {punchHighScore}");
    }
}
