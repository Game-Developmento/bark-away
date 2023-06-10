using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private int targetProgress = 100;
    [SerializeField] private int currProgress = 0;
    private int startPosProgress;
    private int divideRatio = 100;
    [SerializeField] private float duration = 2.5f;

    public Image mask;
    public Image fill;
    private Color color;
    private bool isInProgress = false;
    private float progressTimer = 0f;
    // Event is unique for each process bar!
    public event EventHandler OnTimerOverEvent;

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

    public void SetDuration(float newDuration)
    {
        duration = newDuration;
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
            HandleTimerOver();
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

    public void HandleTimerOver()
    {
        isInProgress = false;
        OnTimerOverEvent?.Invoke(this, EventArgs.Empty);
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

    public int GetTimeLeft()
    {
        return (int)(mask.fillAmount * duration);
    }
    void GetCurrentFill()
    {
        float currentOffset = currProgress;
        float fillAmount = currentOffset / divideRatio;
        mask.fillAmount = fillAmount;
        fill.color = color;
    }

    public float GetTotalTime()
    {
        return duration;
    }
}
