using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputManager))]
public class PlayerController : MonoBehaviour
{
    [Header("Controller")]
    private PlayerInputManager playerInputManager;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSmoothTime = 0.15f;
    private float turnSmoothVelocity;
    private Transform cam;

    [Header("Interactions")]
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private int interactLayer = 8;

    [Header("Animations")]
    private Animator animator;
    private int isWalkingHash;
    private int directionHash;
    private int isInteractHash;
    // PRIVATE VARIABLES //
    private List<IInteractable> interactableList = new List<IInteractable>();

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;

        isWalkingHash = Animator.StringToHash("isWalking");
        directionHash = Animator.StringToHash("direction");
        isInteractHash = Animator.StringToHash("isInteract");
    }
    void Start()
    {
        playerInputManager.OnInteractEvent += OnInteractAction;
    }
    // This functions invokes the Interact method when the player presses the keyboard.
    private void OnInteractAction(object sender, System.EventArgs e)
    {
        Debug.Log("Interact!");
        IInteractable interactable = GetInteractableObject();
        if (interactable != null)
        {
            animator.SetTrigger(isInteractHash);
            interactable.Interact(transform);
        }
    }
    // This functions looks for interactable objects in a certain range from the player, and returns the closest one.
    // It is also used for displaying interactables on the UI. 
    public IInteractable GetInteractableObject()
    {
        // Find all interactables near player
        int interactLayerMask = 1 << interactLayer;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange, interactLayerMask);
        foreach (Collider collider in colliderArray)
        {   
            Debug.Log("Found collider!");
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                Debug.Log("Got interactable!");
                interactableList.Add(interactable);
            }
        }
        return GetClosestInteractable();
    }

    private IInteractable GetClosestInteractable()
    {
        // Find closest interactable
        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                float minDist = Vector3.Distance(transform.position, closestInteractable.GetGameObjectTransform().position);
                float currDist = Vector3.Distance(transform.position, interactable.GetGameObjectTransform().position);
                if (currDist < minDist)
                {
                    // Found closer interactable!
                    closestInteractable = interactable;
                }
            }
        }
        interactableList.Clear();
        return closestInteractable;
    }

    void Update()
    {
        HandleAnimations();
        if (playerInputManager.IsMovementPressed())
        {
            Vector3 currentMovement = playerInputManager.GetMovementVectorNormalized();

            // rotate the dog towards the movement direction
            float targetAngle = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f).normalized;

            // move the dog in the movement direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
        }

    }

    private void HandleAnimations()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isMovementPressed = playerInputManager.IsMovementPressed();
        // Determine idle or walking state
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

}
