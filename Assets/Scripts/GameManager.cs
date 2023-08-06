using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMenuMobileScene()
    {
        SceneManager.LoadScene("MainMenu Mobile");
    }
    public void RestartGame()
    {
        string mostRecentScene = PlayerPrefs.GetString("mostRecentScene");
        PlayerPrefs.DeleteKey("mostRecentScene");
        SceneManager.LoadScene(mostRecentScene);
    }

}
