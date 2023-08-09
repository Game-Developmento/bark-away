using UnityEngine;


public class ToggleSound : MonoBehaviour
{

    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    [SerializeField] private AudioSource audioSource;
    private int soundActive = 1;
    private int soundInactive = 0;
    private string soundKey = "sound";


    private void Awake()
    {
        if (!PlayerPrefs.HasKey(soundKey))
        {
            PlayerPrefs.SetInt(soundKey, soundActive);
        }
    }
    private void Start()
    {
        bool isActive = PlayerPrefs.GetInt(soundKey) == soundActive;
        audioSource.Play();
        audioSource.loop = true;
        if (isActive)
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        OnToggleSound();

        // if (!isActive)
        // {
        //     Debug.Log("should pause");
        //     audioSource.Pause();
        // }
    }
    public void OnToggleSound()
    {
        soundOn.SetActive(!soundOn.activeSelf);
        soundOff.SetActive(!soundOff.activeSelf);
        if (soundOn.activeSelf)
        {
            PlayerPrefs.SetInt(soundKey, soundActive);
            audioSource.UnPause();
        }
        else
        {
            PlayerPrefs.SetInt(soundKey, soundInactive);
            audioSource.Pause();
        }
    }
}