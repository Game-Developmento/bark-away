using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    // Events for input actions
    public event EventHandler OnInteractEventPerformed;
    public event EventHandler OnInteractEventStarted;
    public event EventHandler OnInteractEventCanceled;
    // Event for movement key
    public event EventHandler<MovementKeyEventArgs> OnMovementEventStarted;
    public class MovementKeyEventArgs : EventArgs
    {
        public string keyName;
    }

    // PRIVATE VARIABLES //
    private PlayerInputActions playerInputActions;
    // Player.Move variables
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;

    private void Awake()
    {
        if (playerInputActions == null)
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();

            playerInputActions.Player.Move.started += Movement_started;
            playerInputActions.Player.Move.started += OnMovementInput;
            playerInputActions.Player.Move.canceled += OnMovementInput;
            playerInputActions.Player.Move.performed += OnMovementInput;
            playerInputActions.Player.Interact.started += Interact_started;
            playerInputActions.Player.Interact.performed += Interact_performed;
            playerInputActions.Player.Interact.canceled += Interact_canceled;
        }
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractEventPerformed?.Invoke(this, EventArgs.Empty);
        playerInputActions.Player.Move.Enable();
    }
    private void Interact_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractEventStarted?.Invoke(this, EventArgs.Empty);
        playerInputActions.Player.Move.Disable();
    }
    private void Interact_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractEventCanceled?.Invoke(this, EventArgs.Empty);
        playerInputActions.Player.Move.Enable();
    }
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.y = 0.0f;
        currentMovement.z = currentMovementInput.y;        // This vector is normalized in the input system //
        currentMovement = currentMovement.normalized;
        isMovementPressed = (currentMovement != Vector3.zero) && (currentMovement.magnitude >= 0.1f);
    }

    private void Movement_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        string keyName = playerInputActions.Player.Move.activeControl.displayName;
        OnMovementEventStarted?.Invoke(this, new MovementKeyEventArgs { keyName = keyName });
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }
    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }
    public Vector3 GetMovementVectorNormalized()
    {
        return currentMovement;
    }

    public bool IsMovementPressed()
    {
        return isMovementPressed;
    }
}
