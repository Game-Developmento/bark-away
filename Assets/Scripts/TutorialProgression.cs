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
        beforeGameTutorialDialog[currentTutorialIndex].SetActive(false);
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
                    inGameTutorialDialog[currentTutorialIndex].SetActive(false);
                    currentTutorialIndex++;
                    isTutorialActive = false;
                    ContinueGame();
                }
            }
        }
    }

    public bool IsBeforeGameTutorialFinished()
    {
        return isBeforeGameTutorialFinished;
    }
    private void HandleBeforeGameTutorial()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTutorialIndex++;
            if (currentTutorialIndex < beforeGameTutorialDialog.Length)
            {
                // hide previous tutorial
                beforeGameTutorialDialog[currentTutorialIndex - 1].SetActive(false);
                // show current tutorial
                beforeGameTutorialDialog[currentTutorialIndex].SetActive(true);
            }
            else
            {
                beforeGameTutorialDialog[currentTutorialIndex - 1].SetActive(false);
                EndBeforeGameTutorial();
            }
        }
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
        isTutorialActive = true;
        StopGame();
        if (currentTutorialIndex < inGameTutorialDialog.Length)
        {
            inGameTutorialDialog[currentTutorialIndex].SetActive(true);
        }
    }
}

