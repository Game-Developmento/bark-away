using UnityEngine;

public class FoodInteractable : InteractableBase
{
    public override void StartInteraction(GameObject player) { }
    public override void CancelInteraction(GameObject player) { }
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
        Destroy(gameObject);
    }
}
