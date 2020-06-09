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
        mesh.RecalculateTangents();
        FixMeshNormals();
    }

    public void SetColliderMesh()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    void FixMeshNormals()
    {
        Vector3[] normals = mesh.normals;
        List<int> flippedFaces = new List<int>();

        for(int i = 0; i < normals.Length; i++)
        {
            if(normals[i].z > 0)
            {
                normals[i] = new Vector3( 0f, 0f, -1.0f);
                flippedFaces.Add(i);
            }

        }

        mesh.normals = normals;

        for(int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for(int j = 0; j < tris.Length; j += 3)
            {
                if(flippedFaces.Contains(j / 3))
                {
                    int temp = tris[j];
                    tris[j] = tris[j + 1];
                    tris[j + 1] = temp;
                }
            }

            mesh.SetTriangles(tris, i);
        }
    }
}
