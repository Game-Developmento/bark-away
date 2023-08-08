using UnityEngine;


public class ToggleSound : MonoBehaviour
{

    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    [SerializeField] private AudioSource audioSource;
    private int soundActive = 1;
    private int soundInactive = 0;
    private string soundKey = "sound";

    private void Start()
    {
        audioSource.Play();
        audioSource.loop = true;
    }
    public void OnToggleSound()
    {
        soundOn.SetActive(!soundOn.activeSelf);
        soundOff.SetActive(!soundOff.activeSelf);
        if (soundOn.activeSelf)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Pause();
        }
    }
}