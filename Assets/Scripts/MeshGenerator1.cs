using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator1 : MonoBehaviour
{
    public string path;
    //public int feature;
    public GameObject nodePrefab;

    TopoJsonReader topoReader;

    List<GameObject> nodes;

    Mesh mesh;

    Vector3[] verticesTopoJson;

    int[] triangles;

    void Start()
    {
        nodes = new List<GameObject>();
        //mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;

        topoReader = new TopoJsonReader();
        topoReader.ParseTopoJSON(path);

        SetGameObjectScale();

        //verticesTopoJson = topoReader.

        //CreateShape();
        CreateAllGameObjects();

        //CreateShapeFromTopoJSON();
        //UpdateMesh();
    }

    void CreateAllGameObjects()
    {
        for(int i = 0; i < topoReader.RetrieveJsonData().objects.collection.geometries.Count; i++)
        {
            //Instantiate nodePrefab at the same position as the parentNode
            GameObject newNode = Instantiate(nodePrefab, gameObject.transform.position, Quaternion.identity);
            CreateShapeFromTopoJSON(i);

            newNode.GetComponent<Node>().SetNewVerticesFromTopoJSON(verticesTopoJson);
            newNode.GetComponent<Node>().SetNewTrianglesFromTopoJSON(triangles);
            newNode.GetComponent<Node>().UpdateMesh();
            newNode.transform.parent = this.transform;

            nodes.Add(newNode);
        }
    }

    void SetGameObjectScale() {
        GetComponent<Transform>().localScale = 
            new Vector3(
                    topoReader.RetrieveJsonData().transform.scale[0],
                    topoReader.RetrieveJsonData().transform.scale[1],
                    1
                );
    }

    void CreateShapeFromTopoJSON(int feature)
    {
        topoJsonWrapper topoData = topoReader.RetrieveJsonData();
        //int todoArc = topoData.objects.collection.geometries[0].arcs[0];

        verticesTopoJson = new Vector3[topoData.arcs[feature].Count-1]; //Change arcs to those of geometry
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

    }

    /*void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verticesTopoJson;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }*/
}
