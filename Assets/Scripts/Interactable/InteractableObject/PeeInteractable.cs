using UnityEngine;
using System.Collections;

public class PeeInteractable : InteractableBase
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private GameObject nearestPlayer; // Set nearest player during interactions
    private Animator playerAnimator;
    [SerializeField] private float destoryPeeDelay = 15f;
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
            Vector3 positionToSpawn = new Vector3(nearestPlayerPosition.x, transform.position.y, nearestPlayerPosition.z);
            GameObject spawnedPee = Instantiate(prefabToSpawn, positionToSpawn, prefabToSpawn.transform.rotation);
            StartCoroutine(DestroyPee(spawnedPee.gameObject));
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            // TODO: Check why this works only in children
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

    private IEnumerator DestroyPee(GameObject spawnedPee)
    {
        yield return new WaitForSeconds(destoryPeeDelay);
        Destroy(spawnedPee);
        Destroy(gameObject);
    }
}
