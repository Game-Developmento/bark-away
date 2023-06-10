using UnityEngine;

public class MovementTask : MonoBehaviour
{
    [SerializeField] string keyName;
    PlayerController playerController;
    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.OnPlayerMovementEvent += PlayerController_OnMovementPressed;
        }
    }

    private void PlayerController_OnMovementPressed(object sender, PlayerController.KeyNameEventArgs E)
    {
        string keyPressed = E.name;
        if (keyPressed == keyName && Time.timeScale == 1)
        {
            // Removes only the task that matches the key that was pressed
            TaskManager.Instance.RegularTaskCompleted(gameObject.GetInstanceID());
        }
    }
}