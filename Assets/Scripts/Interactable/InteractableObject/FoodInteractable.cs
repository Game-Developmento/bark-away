using UnityEngine;

public class FoodInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Food Interact!");
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
