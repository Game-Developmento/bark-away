using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        // need to check from which scene we came from
        SceneManager.LoadScene("Ori BarkAway V3");
    }
 
}
