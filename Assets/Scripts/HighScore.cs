using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HighScore : MonoBehaviour
{
    [SerializeField] private TextMeshPro level1HighScoreText = null;
    [SerializeField] private TextMeshPro level2HighScoreText = null;
    [SerializeField] private TextMeshPro level3HighScoreText = null;
    
    private void Start() {
        if (level1HighScoreText.text != null && PlayerPrefs.HasKey("Mobile Level 1_highScore")) {
            int highScore = PlayerPrefs.GetInt("Mobile Level 1_highScore");
            level1HighScoreText.text = "LEVEL 1     BEST: " + highScore;
        }
        if (level2HighScoreText.text != null && PlayerPrefs.HasKey("Mobile Level 2_highScore")) {
            int highScore = PlayerPrefs.GetInt("Mobile Level 2_highScore");
            level2HighScoreText.text = "LEVEL 2     BEST: " + highScore;
        }
        if (level3HighScoreText.text != null && PlayerPrefs.HasKey("Mobile Level 3_highScore")) {
            int highScore = PlayerPrefs.GetInt("Mobile Level 3_highScore");
            level3HighScoreText.text = "LEVEL 3     BEST: " + highScore;
        }
    }
}
