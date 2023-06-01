using UnityEngine;

public class PeeInteractable : InteractableBase
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private GameObject nearestPlayer; // Set nearest player during interactions
    private Animator playerAnimator;
    public override void StartInteraction(GameObject player)
    {
        // Initilalize current player variables
        nearestPlayer = player;
        playerAnimator = nearestPlayer?.GetComponent<Animator>();
        // Start interaction animation
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("startPeeing");
        }
    }
    public override void CancelInteraction(GameObject player)
    {
        Debug.Log("CancelInteracton!");
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("cancelPeeing");
        }
        // Reset variables for next time
        nearestPlayer = null;
        playerAnimator = null;
    }
    public override void Interact(GameObject player)
    {
        Debug.Log("Interact PeeInteractable!");
        TaskManager taskManager = TaskManager.Instance;
        if (taskManager != null)
        {
            bool isTaskCompleted = taskManager.IsTaskCompleted(this);
        }
    }
    public override void Cleanup()
    {
        Debug.Log("Cleanup PeeInteractable!");
        Vector3 nearestPlayerPosition = nearestPlayer.transform.position;
        Vector3 positionToSpawn = new Vector3(nearestPlayerPosition.x, transform.position.y, nearestPlayerPosition.z);
        Instantiate(prefabToSpawn, positionToSpawn, prefabToSpawn.transform.rotation);
        Destroy(gameObject);
    }
}
