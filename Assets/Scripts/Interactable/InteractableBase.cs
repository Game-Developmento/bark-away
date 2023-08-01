using UnityEngine;

// Interactable objects implement this base class!
// Note: Interactable objects must have a collider so interactions can be detected!
public abstract class InteractableBase : MonoBehaviour
{
    [SerializeField] private string interactText;
    [SerializeField] protected bool isCurrentlyInteracting;

    // Absract methods: Derived classes must implement these methods!
    public abstract void StartInteraction(GameObject player);
    public abstract void CancelInteraction(GameObject player);
    public abstract void Interact(GameObject player);
    public abstract void Cleanup();
    // Virtual methods: Derived classes can optionally override default behavior.
    public virtual InteractableBase Initialize() { return this; }
    public virtual string GetInteractText() { return interactText; }
    public virtual Transform GetGameObjectTransform() { return gameObject.transform; }
    public virtual int GetGameObjectID() { return gameObject.GetInstanceID(); }
    public virtual bool IsCurrentlyInteracting() { return isCurrentlyInteracting; }
}
