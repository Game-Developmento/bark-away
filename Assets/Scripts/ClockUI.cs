using UnityEngine;
using TMPro;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private Transform clockHourHandTransform;
    [SerializeField] private TextMeshProUGUI timeText;

    private float clockRotationSpeed = 6f; // Degrees per second for clock rotation
    private float startTime;
    private float secondsPerHour = 3600f;
    private float secondsPerMinute = 60f;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        clockHourHandTransform.Rotate(Vector3.forward, -clockRotationSpeed * Time.deltaTime);

        // Time calculation
        float timePassed = Time.time - startTime;
        int minutes = Mathf.FloorToInt((timePassed % secondsPerHour) / secondsPerMinute);
        int seconds = Mathf.FloorToInt(timePassed % secondsPerMinute);

        string minutesString = minutes.ToString("00");
        string secondsString = seconds.ToString("00");

        timeText.text = minutesString + ":" + secondsString;
    }
}
