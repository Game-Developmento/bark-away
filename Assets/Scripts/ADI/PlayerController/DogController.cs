
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class DogController : MonoBehaviour
{
    [Header("Keyboard Input")]
    private DogInputActions dogInputActions;

    [SerializeField] Vector2 currentMovementInput;
    [SerializeField] Vector3 currentMovement;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 0.5f;

    private int isWalkingHash;
    private int directionHash;
    private int isInteractHash;


    [Header("Animations")]
    private Animator animator;
    [SerializeField] private bool isMovementPressed;
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isVertical;
    private enum direction
    {
        Left = -1,
        Forward = 0,
        Right = 1,

    };
    [SerializeField] private bool isTurningRight;
    [SerializeField] private bool isTurningLeft;

    private void Awake()
    {
        dogInputActions = new DogInputActions();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        directionHash = Animator.StringToHash("direction");
        isInteractHash = Animator.StringToHash("isInteract");

        dogInputActions.Player.Move.started += OnMovementInput;
        dogInputActions.Player.Move.canceled += OnMovementInput;
        dogInputActions.Player.Move.performed += OnMovementInput;

        dogInputActions.Player.Interact.performed += OnInteractInput;
    }

    private void OnEnable()
    {
        dogInputActions.Player.Enable();
    }
    private void OnDisable()
    {
        dogInputActions.Player.Disable();
    }
    private void Update()
    {
        HandleRotation();
        HandleAnimations();
        if (isMovementPressed)
        {
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentMovement), rotationSpeed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, currentMovement, Time.deltaTime * rotationSpeed);
            transform.position += currentMovement * moveSpeed * Time.deltaTime;
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.y = 0.0f;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovement != Vector3.zero;
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        Debug.Log("Interact!");
        animator.SetTrigger(isInteractHash);
    }

    private void HandleRotation()
    {
        isTurningRight = Input.GetAxis("Horizontal") > 0;
        isTurningLeft = Input.GetAxis("Horizontal") < 0;
        isVertical = Input.GetAxis("Vertical") != 0; // true if going forwards or backwards
    }
    private void HandleAnimations()
    {
        isWalking = animator.GetBool(isWalkingHash);
        // Determine idle or walking state
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
            animator.SetInteger(directionHash, (int)direction.Forward);

        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // Handle walking direction
        // if (isWalking)
        // {
        // if (isTurningRight && !isVertical)
        // {
        //     animator.SetInteger(directionHash, (int)direction.Right);
        // }
        // else if (isTurningLeft && !isVertical)
        // {
        //     animator.SetInteger(directionHash, (int)direction.Left);
        // }
        // else
        // {
        //     animator.SetInteger(directionHash, (int)direction.Forward);
        // }
        // }
    }
}
