using UnityEngine;

// Interactable objects implement this interface!
// Note: Interactable objects must have a collider so interactions can be detected!
public interface IInteractable
{
    void StartInteraction();
    void CancelInteraction();
    void Interact(Transform interactorTransform);
    IInteractable Initialize();
    void Cleanup();
    string GetInteractText();
    Transform GetGameObjectTransform();
    int GetGameObjectID();
}
