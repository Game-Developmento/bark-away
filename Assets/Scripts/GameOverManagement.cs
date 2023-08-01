using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverManagement : MonoBehaviour
{
    private int highScore;
    public void GameOver(int totalPoints, int taskCompleted, int fastestTaskCompleted, string sceneToLoad)
    {
        // Store the values using PlayerPrefs
        PlayerPrefs.SetInt("totalPoints", totalPoints);
        PlayerPrefs.SetInt("tasksCompleted", taskCompleted);
        PlayerPrefs.SetInt("fastestTaskCompleted", fastestTaskCompleted);
        PlayerPrefs.SetString("mostRecentScene", SceneManager.GetActiveScene().name);
        UpdateHighScore(totalPoints);

        // Load the "GameOver" scene
        SceneManager.LoadScene(sceneToLoad);
    }

    private void UpdateHighScore(int totalPoints)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!TaskManager.Instance.IsTutorialScene())
        {
            string highScoreKey = sceneName + "_highScore";
            if (PlayerPrefs.HasKey(highScoreKey))
            {
                int currentHighestScore = PlayerPrefs.GetInt(highScoreKey);
                PlayerPrefs.SetInt(highScoreKey, Mathf.Max(currentHighestScore, totalPoints));
            }
            else
            {
                PlayerPrefs.SetInt(highScoreKey, totalPoints);
            }
        }
    }
}
