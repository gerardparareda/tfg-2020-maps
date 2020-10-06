using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Mesh mesh;
    LineRenderer border;
    Vector3[] borderPoints;
    public Material borderMat;
    public Color selectedBorderColor;
    public float borderWidth = 0.01f;

    public Province province;

    Vector3[] newVerticesTopoJson;

    int[] newTriangles;
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        border = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        transform.localScale = transform.localScale;
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

        //Arreglats enclavaments movent-los una mica endavant per solucionar el Z-Fighting
        if (mesh.bounds.size.x < 0.09f)
        {
            Vector3[] tmpVerts = new Vector3[newVerticesTopoJson.Length];

            for(int i = 0; i < mesh.vertices.Length; i++)
            {
                tmpVerts[i] = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, -0.001f);
            }

            mesh.vertices = tmpVerts;
        }

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        FixMeshNormals();
        SetOutline();
    }

    public void SetColliderMesh()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void SetOutline()
    {
        borderPoints = new Vector3[mesh.vertices.Length+1]; //+1 To add the closing one
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            borderPoints[i] = mesh.vertices[i];
        }
        borderPoints[borderPoints.Length-1] = borderPoints[0]; //Set the last equal the first to close the border
        border.positionCount = mesh.vertices.Length + 1;
        border.widthMultiplier = borderWidth;
        border.material = borderMat;
        border.SetPositions(borderPoints);
    }

    public void SetOutlineSelected()
    {
        Vector3[] newBorderPos = new Vector3[borderPoints.Length];
        for(int i = 0; i < borderPoints.Length; i++)
        {
            newBorderPos[i] = new Vector3(borderPoints[i].x, borderPoints[i].y, borderPoints[i].z - 0.002f);
        }
        border.SetPositions(newBorderPos);
        border.widthMultiplier = borderWidth + 0.0025f;
        border.material.SetColor("_Color", selectedBorderColor);
        
    }

    public void UnsetOutlineSelected()
    {
        border.SetPositions(borderPoints);
        border.widthMultiplier = borderWidth;
        border.material.SetColor("_Color", Color.black);
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
