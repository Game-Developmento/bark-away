using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewMesh : MonoBehaviour
{
    public GameObject meshObject;
    private MeshFilter meshFilter;
    private FieldOfView fieldOfView;

    private void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        meshFilter = meshObject.GetComponent<MeshFilter>();

        MeshRenderer viewMeshRenderer = meshObject.GetComponent<MeshRenderer>();
        if (viewMeshRenderer == null)
        {
            viewMeshRenderer = meshObject.AddComponent<MeshRenderer>();
        }
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.name = "FieldOfViewMesh";
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(fieldOfView.GetViewAngle() * fieldOfView.GetEdgeResolveIterations());
        float stepAngleSize = fieldOfView.GetViewAngle() / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        FieldOfView.ViewCastInfo oldViewCast = new FieldOfView.ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - fieldOfView.GetViewAngle() / 2 + stepAngleSize * i;
            FieldOfView.ViewCastInfo newViewCast = fieldOfView.ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) >fieldOfView.GetEdgeDistanceTreshold();
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    FieldOfView.EdgeInfo edge = fieldOfView.FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        Mesh viewMesh = meshFilter.mesh;
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

}
