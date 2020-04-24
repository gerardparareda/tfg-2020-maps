using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Mesh mesh;

    Vector3[] newVerticesTopoJson;

    int[] newTriangles;
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetNewVerticesFromTopoJSON(Vector3[] newVerts)
    {
        this.newVerticesTopoJson = newVerts;
    }

    public void SetNewTrianglesFromTopoJSON(int[] triangles)
    {
        this.newTriangles = triangles;
    }
    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = newVerticesTopoJson;
        mesh.triangles = newTriangles;

        mesh.RecalculateNormals();
    }
}
