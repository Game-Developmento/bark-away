using UnityEngine;

public class PeeInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private GameObject prefabToSpawn;
    private Animator playerAnimator;
    public void StartInteraction()
    {
        Debug.Log("StartInteracton!");
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("startPeeing");
        }
    }
    public void CancelInteraction()
    {
        Debug.Log("CancelInteracton!");
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("cancelPeeing");
        }
    }
    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Interact PeeInteractable!");
        TaskManager taskManager = TaskManager.Instance;
        if (taskManager != null)
        {
            bool isTaskCompleted = taskManager.IsTaskCompleted(this);
        }
    }
    public IInteractable Initialize()
    {
        Debug.Log("Initialize PeeInteractable!");
        // Find GameObject named Player, then get animations controller to trigger peeing animations
        GameObject.Find("Player").TryGetComponent(out Animator animator);
        playerAnimator = animator;
        return this;
    }
    public void Cleanup()
    {
        Debug.Log("Cleanup PeeInteractable!");
        Instantiate(prefabToSpawn, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }

    public int GetGameObjectID()
    {
        return gameObject.GetInstanceID();
    }

    public Transform GetGameObjectTransform()
    {
        return gameObject.transform;
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
