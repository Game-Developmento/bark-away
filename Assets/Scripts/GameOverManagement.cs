using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManagement : MonoBehaviour
{

    public void GameOver(int totalPoints, int taskCompleted, int fastestTaskCompleted)
    {
        // Store the values using PlayerPrefs
        PlayerPrefs.SetInt("totalPoints", totalPoints);
        PlayerPrefs.SetInt("tasksCompleted", taskCompleted);
        PlayerPrefs.SetInt("fastestTaskCompleted", fastestTaskCompleted);
        PlayerPrefs.SetString("mostRecentScene", SceneManager.GetActiveScene().name);

        // Load the "GameOver" scene
        SceneManager.LoadScene("GameOver");
    }
}
