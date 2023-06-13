using UnityEngine;
using System.Collections;

public class PoopInteractable : InteractableBase
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private GameObject nearestPlayer; // Set nearest player during interactions
    private Animator playerAnimator;
    private Transform poopSpawnLocation;
    [SerializeField] private float destroyPoopDelay = 15f;
    public override void StartInteraction(GameObject player)
    {
        // Initilalize current player variables
        nearestPlayer = player;
        playerAnimator = nearestPlayer?.GetComponent<Animator>();
        // Start interaction animation
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("startPooping");
        }
        PlayerController playerController = nearestPlayer?.GetComponent<PlayerController>();
        if (playerController != null)
        {
            poopSpawnLocation = playerController.GetPoopSpawnLocation();
        }
    }
    public override void CancelInteraction(GameObject player)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("cancelPooping");
        }
        // Reset variables for next time
        nearestPlayer = null;
        playerAnimator = null;
    }
    public override void Interact(GameObject player)
    {
        TaskManager taskManager = TaskManager.Instance;
        if (taskManager != null)
        {
            bool isTaskCompleted = taskManager.IsInteractableTaskCompleted(this);
        }
    }
    public override void Cleanup()
    {
        if (nearestPlayer != null)
        {
            Vector3 nearestPlayerPosition = nearestPlayer.transform.position;
            Vector3 positionToSpawn;
            if (poopSpawnLocation != null)
            {
                // Player has marker location!
                positionToSpawn = poopSpawnLocation.position;
            }
            else
            {
                // Fallback location
                positionToSpawn = new Vector3(nearestPlayerPosition.x, transform.position.y, nearestPlayerPosition.z);
            }
            GameObject spawnedPoop = Instantiate(prefabToSpawn, positionToSpawn, prefabToSpawn.transform.rotation);
            StartCoroutine(DestroyPoop(spawnedPoop.gameObject));
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            SpriteRenderer[] rendererComponents = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in rendererComponents)
            {
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
        else
        {
            // No player nearby, destory object immediately
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyPoop(GameObject spawnedPoop)
    {
        yield return new WaitForSeconds(destroyPoopDelay);
        Destroy(spawnedPoop);
        Destroy(gameObject);
    }
}
