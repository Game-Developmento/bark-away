using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OwnerPatrol : MonoBehaviour
{
    public Transform[] wayPoints;
    private int waypointIndex;
    private Vector3 target;
    private NavMeshAgent agent;
    private float waitTime;
    private bool isWaiting;
    [SerializeField] private float minDelay = 2f;
    [SerializeField] private float maxDelay = 5f;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        UpdateDestination();
    }

    private void Update()
    {
        if (isWaiting)
            return;
        bool isCloseEnough = Vector3.Distance(transform.position, target) < 1;
        if (isCloseEnough)
        {
            float nextDelay = Random.Range(minDelay, maxDelay);
            StartCoroutine(WaitBeforeNextDestination(nextDelay));
        }
    }

    private IEnumerator WaitBeforeNextDestination(float delay)
    {
        isWaiting = true;
        yield return new WaitForSeconds(delay);
        isWaiting = false;

        IterateWaypointIndex();
        UpdateDestination();
    }

    private void UpdateDestination()
    {
        target = wayPoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex = (waypointIndex + 1) % wayPoints.Length;
    }
}
