using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private int targetProgress = 100;
    [SerializeField] private int currProgress = 0;
    private int startPosProgress;
    private int divideRatio = 100;
    [SerializeField] private float duration = 3f;

    public Image mask;
    public Image fill;
    private Color color;
    private bool isInProgress = false;
    private float progressTimer = 0f;

    private void Start()
    {
        startPosProgress = currProgress;
    }

    private void Update()
    {
        if (isInProgress)
        {
            HandleProgressBar(startPosProgress);
        }
    }


    private void HandleProgressBar(int startingPoint)
    {
        progressTimer += Time.deltaTime;
        currProgress = (int)Mathf.Lerp(startingPoint, targetProgress, progressTimer / duration);
        GetCurrentFill();

        if (progressTimer >= duration)
        {
            currProgress = targetProgress;
            GetCurrentFill();
            CancelProgress();
        }
    }


    public void BeginProgress()
    {
        if (!isInProgress)
        {
            isInProgress = true;
            progressTimer = 0f;
            gameObject.SetActive(true);
        }
    }

    public void CancelProgress()
    {
        isInProgress = false;
        gameObject.SetActive(false);
        currProgress = startPosProgress;

    }

    public bool IsCurrentlyInProgress()
    {
        return isInProgress;
    }

    public int GetCurrentProgress()
    {
        return currProgress;
    }
    void GetCurrentFill()
    {
        float currentOffset = currProgress;
        float fillAmount = currentOffset / divideRatio;
        mask.fillAmount = fillAmount;
        fill.color = color;
    }
}
