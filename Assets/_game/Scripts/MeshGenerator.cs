using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 meshSize;
    [SerializeField] private int meshResolution;
    [SerializeField] private float curveValue;

    private Mesh targetMesh;
    private MeshFilter meshFilter;

    private List<Vector3> vertices;
    private List<Vector2> uv;
    private List<Vector3> normals;
    private List<int> triangles;

    private void Awake()
    {
        targetMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = targetMesh;
    }

    private void Start()
    {
        GenerateMesh(meshSize, meshResolution);
        AlterateMesh(curveValue);
        AssignMesh();
    }

    private void GenerateMesh(Vector2 size, int resolution)
    {
        vertices = new List<Vector3>();
        uv = new List<Vector2>();
        normals = new List<Vector3>();

        float deltaX = size.x / resolution;
        float deltaY = size.y / resolution;

        for(int y = 0; y < (resolution / 2 * 3) + 1; y++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(x * deltaX, 0, y * deltaY));
                uv.Add(new Vector2(x * 1f / resolution, y * 1f / resolution));
                normals.Add(-Vector3.forward);
            }
        }

        triangles = new List<int>();

        for(int row = 0; row < resolution; row++)
        {
            for (int col = 0; col < resolution; col++)
            {
                int i = (row * resolution) + row + col;

                triangles.Add(i);
                triangles.Add(i + (resolution) + 1);
                triangles.Add(i + (resolution) + 2);

                triangles.Add(i);
                triangles.Add(i + (resolution) + 2);
                triangles.Add(i + 1);
            }
        }
    }

    private void AssignMesh()
    {
        targetMesh.Clear();
        targetMesh.vertices = vertices.ToArray();
        targetMesh.uv = uv.ToArray();
        targetMesh.normals = normals.ToArray();
        targetMesh.triangles = triangles.ToArray();
        targetMesh.RecalculateNormals();
    }

    private void AlterateMesh(float value)
    {
        Vector3 origin = new Vector3(meshSize.x / 2, 0, meshSize.y / 4);

        for(int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            float distance = (vertex - origin).magnitude;
            vertex.y = 0.5f * Mathf.Sin(value + distance);
            vertices[i] = vertex;
        }
    }
}
