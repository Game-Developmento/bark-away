using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalPointsText;
    [SerializeField] private TextMeshProUGUI tasksCompletedText;
    [SerializeField] private TextMeshProUGUI FastestTaskCompletedText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the stored values using PlayerPrefs
        int totalPoints = PlayerPrefs.GetInt("totalPoints");
        int tasksCompleted = PlayerPrefs.GetInt("tasksCompleted");
        int FastestTaskCompleted = PlayerPrefs.GetInt("fastestTaskCompleted");
        int highScore = PlayerPrefs.GetInt("highScore");

        // Update the TextMeshProUGUI components with the retrieved values
        totalPointsText.text = "Total Points: " + totalPoints.ToString();
        tasksCompletedText.text = "Tasks Completed: " + tasksCompleted.ToString();
        FastestTaskCompletedText.text = "Fastest Task Completed: " + FastestTaskCompleted.ToString() + "s";
        highScoreText.text = "High Score: " + highScore;

        // Clear the stored values
        DeleteKeys();
    }

    private void DeleteKeys()
    {
        PlayerPrefs.DeleteKey("totalPoints");
        PlayerPrefs.DeleteKey("tasksCompleted");
        PlayerPrefs.DeleteKey("fastestTaskCompleted");
    }


}
