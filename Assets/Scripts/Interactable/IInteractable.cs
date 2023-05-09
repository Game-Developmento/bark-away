using UnityEngine;

// Interactable objects implement this interface!
// Note: Interactable objects must have a collider so interactions can be detected!
public interface IInteractable
{
    void Interact(Transform interactorTransform);
    string GetInteractText();
    Transform GetTransform();
}
