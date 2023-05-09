using UnityEngine;

public class FoodInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public void Interact(Transform interactorTransform)
    {
        TaskManager taskManager = GetComponentInParent<TaskManager>();
        if (taskManager != null)
        {
            bool isTaskCompleted = taskManager.IsTaskCompleted(this);
            if (isTaskCompleted)
            {
                this.gameObject.SetActive(false);
            }
        }

    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
