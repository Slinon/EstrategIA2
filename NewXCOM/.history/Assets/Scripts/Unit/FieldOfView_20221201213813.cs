using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    private float fov;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate() {
        fov = 367.5f;
        int rayCount = 50;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 5f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;


        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            Vector3 direction = Quaternion.Euler(90, 0, 0) * GetVectorFromAngle(angle);

            // Detect walls 
            RaycastHit hit;
            //RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, viewDistance, layerMask);
            //Debug.Log("collider: " + raycastHit2D.collider.gameObject);
            Debug.DrawRay(origin, direction * viewDistance, Color.red);
            //Debug.Log("ORIGIN: " + origin);
            if(Physics.Raycast(origin, direction, out hit, viewDistance, layerMask) /*raycastHit2D.collider != null*/)
            {
                
                // Hit object
                vertex = hit.point;
                //vertex = raycastHit2D.point;
            }
            else
            {
                // No hit
                vertex =  + direction * viewDistance;
            }
            

            vertices[vertexIndex] = vertex;

            if(i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    // Copied from CodeMonkey utilities
    private Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
        Debug.Log("changing origin (FielOfView.SetOrigin): " + this.origin);
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2f;
    }

    // Copied from CodeMonkey utilities
    private float GetAngleFromVectorFloat(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0) n += 360;

        return n;
    }
       
}
