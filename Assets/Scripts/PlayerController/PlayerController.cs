using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputManager))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Controller")]
    private PlayerInputManager playerInputManager;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSmoothTime = 0.15f;
    private float turnSmoothVelocity;
    [Header("Camera Options")]
    [SerializeField] private bool isFarViewCam = true;
    private Transform cam;
    [SerializeField] private GameObject closeCam;
    [SerializeField] private GameObject farCam;

    [Header("Events")]
    [SerializeField] private ProgressBar progressBar;
    public event EventHandler OnPlayerInteractStarted;
    public event EventHandler OnPlayerInteractCanceled;
    public event EventHandler OnPlayerInteractPerformed;
    public event EventHandler<KeyNameEventArgs> OnPlayerMovementEvent;
    public class KeyNameEventArgs : EventArgs
    {
        public string name;
    }

    [Header("Interactions")]
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private int interactLayer = 8;

    [Header("Animations")]
    private Animator animator;
    private int isWalkingHash;
    private int directionHash;
    // PRIVATE VARIABLES //
    [Header("Interactions")]
    private List<InteractableBase> interactableList = new List<InteractableBase>();
    InteractableBase currInteractable; // The interactable the player was near when started pressing 'E'
    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        isFarViewCam = farCam.activeSelf;

        isWalkingHash = Animator.StringToHash("isWalking");
        directionHash = Animator.StringToHash("direction");
    }
    void Start()
    {
        playerInputManager.OnInteractEventPerformed += OnInteractActionPerformed;
        playerInputManager.OnInteractEventStarted += OnInteractActionStarted;
        playerInputManager.OnInteractEventCanceled += OnInteractActionCanceled;
        playerInputManager.OnMovementEventStarted += OnPlayerMovementStarted;
    }
    private void OnPlayerMovementStarted(object sender, PlayerInputManager.MovementKeyEventArgs E)
    {
        if (E?.keyName != null)
        {
            string keyPressed = E.keyName;
            OnPlayerMovementEvent?.Invoke(this, new KeyNameEventArgs { name = keyPressed });
        }
    }
    private void OnInteractActionStarted(object sender, System.EventArgs e)
    {
        currInteractable = GetInteractableObject();
        if (currInteractable != null)
        {
            if (progressBar != null)
            {
                Debug.Log("Interact Started!");
                progressBar.BeginProgress();
            }
            currInteractable.StartInteraction(gameObject);
            OnPlayerInteractStarted?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnInteractActionCanceled(object sender, System.EventArgs e)
    {
        if (currInteractable != null)
        {
            if (progressBar != null)
            {
                Debug.Log("Interact Canceled!");
                progressBar.HandleTimerOver();
            }
            currInteractable.CancelInteraction(gameObject);
            OnPlayerInteractCanceled?.Invoke(this, EventArgs.Empty);
            currInteractable = null;
        }
    }

    // This functions invokes the Interact method when the player presses the keyboard.
    private void OnInteractActionPerformed(object sender, System.EventArgs e)
    {
        Debug.Log("Interact Perfomed!");

        InteractableBase nearestInteractable = GetInteractableObject();
        // Check if interactable changed since player started pressing 'E'
        if ((nearestInteractable != null && currInteractable != null)
        && (nearestInteractable.GetGameObjectID() == currInteractable.GetGameObjectID()))
        {
            currInteractable.Interact(gameObject);
            OnPlayerInteractPerformed?.Invoke(this, EventArgs.Empty);
        }
        // Reset variables
        currInteractable = null;
    }

    // This functions looks for interactable objects in a certain range from the player, and returns the closest one.
    // It is also used for displaying interactables on the UI. 
    public InteractableBase GetInteractableObject()
    {
        // Find all interactables near player
        int interactLayerMask = 1 << interactLayer;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange, interactLayerMask);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out InteractableBase interactable))
            {
                interactableList.Add(interactable);
            }
        }
        return GetClosestInteractable();
    }

    private InteractableBase GetClosestInteractable()
    {
        // Find closest interactable
        InteractableBase closestInteractable = null;
        foreach (InteractableBase interactable in interactableList)
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
            /* 
            If using free look camera- always rotate.
            If using virtual camera- isFreeLookCam should be toggled false, do not update rotation when walking backwards!
            */
            if (isFarViewCam || currentMovement != Vector3.back)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f).normalized;
            }
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

    public void ToggleCamera()
    {
        closeCam.SetActive(!closeCam.activeSelf);
        farCam.SetActive(!farCam.activeSelf);
        isFarViewCam = farCam.activeSelf;
    }
}
