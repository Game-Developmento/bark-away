using UnityEngine;

public class PeeInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Animator playerAnimator; // Dog animations controller to trigger peeing animation
    
    
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
