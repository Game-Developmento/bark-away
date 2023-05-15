using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private int targetProgress = 100;
    [SerializeField] private int currProgress = 0;

    [SerializeField] private float duration = 3f;

    public Image mask;
    public Image fill;
    private Color color;

    private bool isProgressing = false;
    private float progressTimer = 0f;
    private int startingProgress;

    private void Update()
    {
        if (isProgressing)
        {
            progressTimer += Time.deltaTime;
            float progress = Mathf.Lerp(startingProgress, targetProgress, progressTimer / duration);
            currProgress = (int)progress;
            GetCurrentFill();

            if (progressTimer >= duration)
            {
                currProgress = targetProgress;
                GetCurrentFill();
                isProgressing = false;
                gameObject.SetActive(false);
                currProgress = 0;

            }
        }

    }

    public void LoadProgress()
    {
        if (!isProgressing)
        {
            isProgressing = true;
            startingProgress = currProgress;
            progressTimer = 0f;
            gameObject.SetActive(true);

        }
    }

    public int GetCurrentProgress()
    {
        return currProgress;
    }

    void GetCurrentFill()
    {
        float currentOffset = currProgress;
        float maximumOffset = targetProgress;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
        fill.color = color;
    }
}
