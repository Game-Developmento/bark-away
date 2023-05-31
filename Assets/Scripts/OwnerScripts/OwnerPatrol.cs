using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class OwnerPatrol : MonoBehaviour
{
    public Transform[] wayPoints;
    private int waypointIndex;
    private Vector3 target;
    private NavMeshAgent agent;
    private float waitTime;
    private bool isWaitingForNextDest;
    [SerializeField] private float minDelay = 2f;
    [SerializeField] private float maxDelay = 5f;

    [Header("Animations")]
    private Animator animator;
    private int isWalkingHash;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }
    private void Start()
    {
        UpdateDestination();
    }

    private void Update()
    {
        if (isWaitingForNextDest)
        {
            return;
        }
        bool isCloseEnoughToTarget = Vector3.Distance(transform.position, target) < 1;
        if (isCloseEnoughToTarget)
        {
            float nextDelay = Random.Range(minDelay, maxDelay);
            StartCoroutine(WaitBeforeNextDestination(nextDelay));
            animator.SetBool(isWalkingHash, false); // Set walking animation to false when close to the target
        }
    }

    private IEnumerator WaitBeforeNextDestination(float delay)
    {
        isWaitingForNextDest = true;
        yield return new WaitForSeconds(delay);
        isWaitingForNextDest = false;

        IterateWaypointIndex();
        UpdateDestination();
    }

    private void UpdateDestination()
    {
        target = wayPoints[waypointIndex].position;
        agent.SetDestination(target);
        animator.SetBool(isWalkingHash, true); // Set walking animation to true when not close to the target
    }

    private void IterateWaypointIndex()
    {
        waypointIndex = (waypointIndex + 1) % wayPoints.Length;
    }
}
