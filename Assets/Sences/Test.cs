using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Vector2[] uvs = meshFilter.mesh.uv;
        Vector3[] vertices = meshFilter.mesh.vertices;
        int[] trianles = meshFilter.mesh.triangles;

        Debug.LogError(vertices.Length);

        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.Log(vertices[i]);
        }
    }
}
