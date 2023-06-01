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
        ResetVariables();
    }
    public override void Interact(GameObject player)
    {
        Debug.Log("Interact PeeInteractable!");
        TaskManager taskManager = TaskManager.Instance;
        if (taskManager != null)
        {
            bool isTaskCompleted = taskManager.IsTaskCompleted(this);
        }
        ResetVariables();
    }
    public override void Cleanup()
    {
        Debug.Log("Cleanup PeeInteractable!");
        Instantiate(prefabToSpawn, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
    private void ResetVariables()
    {
        nearestPlayer = null;
        playerAnimator = null;
    }
}
