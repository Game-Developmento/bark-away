using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseGame : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    private bool isGameAlreadyPaused;
    public void LoadMainMenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMenuMobileScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu Mobile");
    }


    public void OnGamePaused()
    {
        isGameAlreadyPaused = Time.timeScale == 0;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void OnGameResumed()
    {
        if (!isGameAlreadyPaused)
        {
            Time.timeScale = 1;
        }
        pauseMenu.SetActive(false);
    }

}