using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator1 : MonoBehaviour
{
    public string path;

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
        
        verticesTopoJson = new Vector3[topoData.arcs[0].Count-1];

        float posX, posY = 0;
        verticesTopoJson[0] = new Vector3(topoData.arcs[0][0].x, topoData.arcs[0][0].y, 0);
        for (int i = 1; i < topoData.arcs[0].Count-1; i++)
        {
            posX = verticesTopoJson[i - 1].x + topoData.arcs[0][i].x;
            posY = verticesTopoJson[i - 1].y + topoData.arcs[0][i].y;
            verticesTopoJson[i] = new Vector3(posX, posY, 0);
            Debug.Log(verticesTopoJson[i]);
        }

        triangles = Delaunay.Triangulate(verticesTopoJson);

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
