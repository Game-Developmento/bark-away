using UnityEngine;
using TMPro;
using System;


public class ClockUI : MonoBehaviour
{
    [SerializeField] private Transform clockHourHandTransform;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float startTimeInSeconds = 60f;
    private float clockRotationSpeed = 6f; // Degrees per second for clock rotation
    private float secondsPerMinute = 60f;

    public event EventHandler OnTimeOverEvent;

    private void Start()
    {
        // Reset the start time when the scene is loaded
        startTimeInSeconds = Time.time + startTimeInSeconds;
    }
    private void Update()
    {
        // Time calculation
        float remainingSeconds = startTimeInSeconds - Time.time;
        if (remainingSeconds > 0)
        {
            clockHourHandTransform.Rotate(Vector3.forward, clockRotationSpeed * Time.deltaTime);
            int minutes = Mathf.FloorToInt(remainingSeconds / secondsPerMinute);
            int seconds = Mathf.FloorToInt(remainingSeconds % secondsPerMinute);

            string minutesString = minutes.ToString("00");
            string secondsString = seconds.ToString("00");

            timeText.text = minutesString + ":" + secondsString;
        }
        else
        {
            timeText.text = "00:00";
            OnTimeOverEvent?.Invoke(this, EventArgs.Empty);
        }
    }

}
