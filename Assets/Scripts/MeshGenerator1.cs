using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator1 : MonoBehaviour
{
    public string path;
    public int feature;

    TopoJsonReader topoReader;

    Mesh mesh;

    Vector3[] vertices;
    Vector3[] verticesTopoJson;

    int[] triangles;
    int[] trianglesTopoJson;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        topoReader = new TopoJsonReader();
        topoReader.ParseTopoJSON(path);

        SetGameObjectScale();

        //verticesTopoJson = topoReader.

        //CreateShape();
        CreateShapeFromTopoJSON();
        UpdateMesh();
    }

    void SetGameObjectScale() {
        GetComponent<Transform>().localScale = 
            new Vector3(
                    topoReader.RetrieveJsonData().transform.scale[0],
                    topoReader.RetrieveJsonData().transform.scale[1],
                    1
                );
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,0,0),
            new Vector3(1,1,0)

        };

        triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2
        };

    }

    void CreateShapeFromTopoJSON()
    {
        topoJsonWrapper topoData = topoReader.RetrieveJsonData();
        int todoArc = topoData.objects.collection.geometries[0].arcs[0];

        verticesTopoJson = new Vector3[topoData.arcs[feature].Count-1];
        Vector2[] vertices2D = new Vector2[verticesTopoJson.Length];
        Vector3[] vertices = new Vector3[vertices2D.Length];
        
        float posX, posY = 0;
        verticesTopoJson[0] = new Vector3(topoData.arcs[feature][0].x, topoData.arcs[feature][0].y, 0);

        for (int i = 1; i < topoData.arcs[feature].Count-1; i++)
        {
            posX = verticesTopoJson[i - 1].x + topoData.arcs[feature][i].x;
            posY = verticesTopoJson[i - 1].y + topoData.arcs[feature][i].y;
            verticesTopoJson[i] = new Vector3(posX, posY, 0);
            Debug.Log(verticesTopoJson[i]);
        }

        for(int i = 0; i < verticesTopoJson.Length; i++)
        {
            vertices2D[i] = new Vector2(verticesTopoJson[i].x, verticesTopoJson[i].y);
        }

        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        verticesTopoJson = vertices;

        triangles = indices;

        //triangles = Delaunay.Triangulate(verticesTopoJson);

        /*triangles = new int[]
        {
            2, 1, 0

        };*/

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verticesTopoJson;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
