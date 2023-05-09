using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;

    [Header("Interactions")]
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private int interactLayer = 8;

    [SerializeField] private GameObject playerUI;



    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }
    void FixedUpdate()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        // float rotateSpeed = 1f;
        // transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    // This functions invokes the Interact method when the player presses the keyboard.
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        IInteractable interactable = GetInteractableObject();
        if (interactable != null)
        {
            interactable.Interact(transform);
        }
    }
    // This functions looks for interactable objects in a certain range from the player, and returns the closest one.
    // It is also used for displaying interactables on the UI. 
    public IInteractable GetInteractableObject()
    {
        // Find all interactables near player
        List<IInteractable> interactableList = new List<IInteractable>();
        int interactLayerMask = 1 << interactLayer;
        Vector3 position = playerUI.transform.position;
        Collider[] colliderArray = Physics.OverlapSphere(position, interactRange, interactLayerMask);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }

        // Find closest interactable
        IInteractable closestinteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestinteractable == null)
            {
                closestinteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                Vector3.Distance(transform.position, closestinteractable.GetTransform().position))
                {
                    // Found closer interactable!
                    closestinteractable = interactable;
                }
            }
        }
        return closestinteractable;
    }

}
