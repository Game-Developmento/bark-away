using UnityEngine;

public class FoodInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public void StartInteraction() { }
    public void CancelInteraction() { }
    public void Interact(Transform interactorTransform)
    {
        TaskManager taskManager = TaskManager.Instance;
        if (taskManager != null)
        {
            bool isTaskCompleted = taskManager.IsTaskCompleted(this);
        }
    }
    public IInteractable Initialize()
    {
        return this;
    }

    public void Cleanup()
    {
        Destroy(gameObject);
    }
    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetGameObjectTransform()
    {
        return gameObject.transform;
    }
    public int GetGameObjectID()
    {
        return gameObject.GetInstanceID();
    }
}
