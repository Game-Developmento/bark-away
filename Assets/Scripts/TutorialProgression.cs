using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialProgression : MonoBehaviour
{

    public static TutorialProgression Instance { get; private set; }

    [SerializeField] private GameObject[] beforeGameTutorialDialog;
    [SerializeField] private GameObject[] inGameTutorialDialog;
    private int currentTutorialIndex = 0;

    private bool isTutorialActive = true;

    private bool isBeforeGameTutorialFinished;

    private Image tutorialUI;
    private void Awake()
    {
        Instance = this;
        tutorialUI = GetComponentInChildren<Image>();
    }
    private void Start()
    {
        Time.timeScale = 0f; // Pause the game initially
        ShowNewDialog(currentTutorialIndex);
    }
    private void Update()
    {
        if (!isTutorialActive)
        {
            return;
        }
        if (!isBeforeGameTutorialFinished)
        {
            HandleBeforeGameTutorial();
        }
        else
        {
            // if we enabled the tutorial on the screen we wait for space key to continue the game and disable it.
            if (tutorialUI.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    HidePriorDialog(currentTutorialIndex);
                    ContinueGame();
                }
            }
        }

    }
    private void HandleBeforeGameTutorial()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTutorialIndex++;
            if (currentTutorialIndex < beforeGameTutorialDialog.Length)
            {
                HidePriorDialog(currentTutorialIndex - 1);
                ShowNewDialog(currentTutorialIndex);
            }
            else
            {
                HidePriorDialog(currentTutorialIndex - 1);
                EndBeforeGameTutorial();
            }
        }
    }
    private void ShowNewDialog(int index)
    {
        beforeGameTutorialDialog[index].SetActive(true);
    }

    private void HidePriorDialog(int index)
    {
        beforeGameTutorialDialog[index].SetActive(false);
    }

    private void EndBeforeGameTutorial()
    {
        isTutorialActive = false;
        isBeforeGameTutorialFinished = true;
        ContinueGame();
        currentTutorialIndex = 0;
    }

    private void StopGame()
    {
        Time.timeScale = 0f;
        tutorialUI.gameObject.SetActive(true);
    }

    private void ContinueGame()
    {
        Time.timeScale = 1f;
        tutorialUI.gameObject.SetActive(false);
    }

    public void HandleNextPartInTutorial()
    {
        StopGame();
        currentTutorialIndex++;
        if (currentTutorialIndex < inGameTutorialDialog.Length)
        {
            ShowNewDialog(currentTutorialIndex);
        }
    }
}

