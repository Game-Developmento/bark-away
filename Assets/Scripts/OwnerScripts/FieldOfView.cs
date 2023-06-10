using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FieldOfView : MonoBehaviour
{

    public event EventHandler OnPlayerInFieldOfView;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private float viewRadius;
    [Range(0, 360)]
    [SerializeField] private float viewAngle;
    [SerializeField] private float delay = .2f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [SerializeField] private float rayCastHeight = 1f;
    [SerializeField] private int edgeResolveIterations = 4;
    [SerializeField] private float edgeDistanceThreshold = 0.5f;
    public Transform target;

    private bool isInteracting;

    private List<Transform> visibleTargets = new List<Transform>();


    private void Awake()
    {
        playerController.OnPlayerInteractStarted += IsInteractStarted;
        playerController.OnPlayerInteractCanceled += IsInteractFinished;
        playerController.OnPlayerInteractPerformed += IsInteractFinished;
    }

    private void IsInteractStarted(object sender, System.EventArgs e)
    {
        isInteracting = true;
    }

    private void IsInteractFinished(object sender, System.EventArgs e)
    {
        isInteracting = false;
    }
    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(delay));
    }

    private void Update()
    {
        if (visibleTargets.Contains(target) && isInteracting)
        {
            OnPlayerInFieldOfView?.Invoke(this, EventArgs.Empty);
        }
    }

    // Find the edge between two view cast points
    public EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    // Perform the view cast at a given angle
    public ViewCastInfo ViewCast(float angle)
    {
        Vector3 direction = DirectionFromAngle(angle, true);
        Vector3 origin = transform.position + Vector3.up * rayCastHeight; // Adjust the eye height as needed
        RaycastHit hit;

        // Perform the raycast and check for a hit
        if (Physics.Raycast(origin, direction, out hit, viewRadius, obstacleMask))
        {
            if (hit.point.y <= origin.y) // Adjust the specific height condition as per your requirement
            {
                return new ViewCastInfo(true, hit.point, hit.distance, angle);
            }
        }

        return new ViewCastInfo(false, origin + direction * viewRadius, viewRadius, angle);
    }


    // Coroutine to find visible targets at regular intervals
    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    // Find the visible targets within the field of view
    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                // Check for obstacles between the transform and the target
                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    // Convert an angle to a direction vector
    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    // Getters
    public float GetViewRadius()
    {
        return viewRadius;
    }

    public float GetViewAngle()
    {
        return viewAngle;
    }

    public int GetEdgeResolveIterations()
    {
        return edgeResolveIterations;
    }

    public float GetEdgeDistanceTreshold()
    {
        return edgeDistanceThreshold;
    }

    // Struct to store information about a view cast
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }

    // Struct to store information about an edge
    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}
