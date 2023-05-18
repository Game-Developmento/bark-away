using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    // Events for input actions
    public event EventHandler OnInteractEvent;

    // PRIVATE VARIABLES //
    private PlayerInputActions playerInputActions;
    // Player.Move variables
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.started += OnMovementInput;
        playerInputActions.Player.Move.canceled += OnMovementInput;
        playerInputActions.Player.Move.performed += OnMovementInput;
        playerInputActions.Player.Interact.performed += Interact_performed;
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractEvent?.Invoke(this, EventArgs.Empty);
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
