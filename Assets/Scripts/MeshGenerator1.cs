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

    Vector2 pos;

    Mesh mesh;

    Vector3[] verticesTopoJson;

    int[] triangles;

    void Start()
    {
        nodes = new List<GameObject>();

        topoReader = new TopoJsonReader();
        topoReader.ParseTopoJSON(path);

        pos = new Vector2(0, 0);

        CreateAllGameObjects();

    }

    void CreateAllGameObjects()
    {

        topoJsonWrapper topoData = topoReader.RetrieveJsonData();

        for (int i = 0; i < topoData.objects.collection.geometries.Count; i++)
        {

            if (topoData.objects.collection.geometries[i].type == "Polygon")
            {
                //Instantiate nodePrefab at the same position as the parentNode
                GameObject newNode = Instantiate(nodePrefab, gameObject.transform.position, Quaternion.identity);
                CreateGeometryFromTopoJSON(i, 0);

                newNode.GetComponent<Node>().SetNewVerticesFromTopoJSON(verticesTopoJson);
                newNode.GetComponent<Node>().SetNewTrianglesFromTopoJSON(triangles);
                newNode.GetComponent<Node>().UpdateMesh();
                newNode.name = "Node" + i;
                newNode.transform.parent = this.transform;

                nodes.Add(newNode);
            }
            if (topoData.objects.collection.geometries[i].type == "MultiPolygon")
            {
                GameObject newNodeParent = Instantiate(nodePrefab, gameObject.transform.position, Quaternion.identity);

                for (int j = 0; j < topoData.objects.collection.geometries[i].arcs.Count; j++)
                {
                    GameObject newNodeChild = Instantiate(nodePrefab, gameObject.transform.position, Quaternion.identity);

                    CreateGeometryFromTopoJSON(i, j);

                    newNodeChild.GetComponent<Node>().SetNewVerticesFromTopoJSON(verticesTopoJson);
                    newNodeChild.GetComponent<Node>().SetNewTrianglesFromTopoJSON(triangles);
                    newNodeChild.GetComponent<Node>().UpdateMesh();
                    newNodeChild.name = "Node" + i + " , " + j;

                    newNodeChild.transform.parent = newNodeParent.transform;
                }

                newNodeParent.name = "Node" + i;
                newNodeParent.transform.parent = this.transform;

                nodes.Add(newNodeParent);
            }
            
        }
    }

    void CreateGeometryFromTopoJSON(int feature, int child)
    {
        topoJsonWrapper topoData = topoReader.RetrieveJsonData();

        List<Vector2> vertices2DList = new List<Vector2>();

        for (int arcFeature = 0; arcFeature < topoData.objects.collection.geometries[feature].arcs[child].Count; arcFeature++)
        {
            if (topoData.objects.collection.geometries[feature].arcs[child][arcFeature] < 0)
            {

                if (arcFeature == 0)
                {
                    for (int arcVertDisplace = 0; arcVertDisplace < topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])].Count - 1; arcVertDisplace++)
                    {

                        if (arcFeature > 0 && arcVertDisplace == 0)
                        {
                            continue;
                        }

                        if (arcFeature == 0 && arcVertDisplace == 0)
                        {

                            pos.x = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];

                        }
                        else
                        {
                            pos.x += topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y += topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];

                        }

                        vertices2DList.Add(new Vector2(pos.x, pos.y));
                    }
                    vertices2DList.Reverse();
                    pos.x = vertices2DList[vertices2DList.Count - 1].x;
                    pos.y = vertices2DList[vertices2DList.Count - 1].y;
                }
                else
                {
                    int tmpArcFeat = topoData.objects.collection.geometries[feature].arcs[child][arcFeature];
                    for (int arcVertDisplace = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])].Count - 1; arcVertDisplace > 0; arcVertDisplace--)
                    {

                        //Saltar-se la primera delta dels arcFeature quan ja no és la primera feature
                        if (arcFeature == 0 && arcVertDisplace == topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])].Count - 1)
                        {
                            pos.x = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];
                        }
                        else
                        {
                            pos.x -= topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y -= topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[child][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];

                        }

                        vertices2DList.Add(new Vector2(pos.x, pos.y));

                    }
                }


            }
            else
            {
                int tmpArcFeat = topoData.objects.collection.geometries[feature].arcs[child][arcFeature];
                for (int arcVertDisplace = 0; arcVertDisplace < topoData.arcs[topoData.objects.collection.geometries[feature].arcs[child][arcFeature]].Count; arcVertDisplace++)
                {

                    if (arcFeature > 0 && arcVertDisplace == 0)
                    {
                        continue;
                    }

                    if (arcFeature == 0 && arcVertDisplace == 0)
                    {
                        pos.x = topoData.arcs[topoData.objects.collection.geometries[feature].arcs[child][arcFeature]][arcVertDisplace].x * topoData.transform.scale[0];
                        pos.y = topoData.arcs[topoData.objects.collection.geometries[feature].arcs[child][arcFeature]][arcVertDisplace].y * topoData.transform.scale[1];

                    }
                    else
                    {
                        pos.x += topoData.arcs[topoData.objects.collection.geometries[feature].arcs[child][arcFeature]][arcVertDisplace].x * topoData.transform.scale[0];
                        pos.y += topoData.arcs[topoData.objects.collection.geometries[feature].arcs[child][arcFeature]][arcVertDisplace].y * topoData.transform.scale[1];

                    }

                    vertices2DList.Add(new Vector2(pos.x, pos.y));
                }
            }
        }


        if (feature == 19)
        {

        }

        Vector2[] vertices2D = new Vector2[vertices2DList.Count];
        Vector3[] vertices = new Vector3[vertices2D.Length];

        for (int i = 0; i < vertices2DList.Count; i++)
        {
            vertices2D[i] = new Vector2(vertices2DList[i].x, vertices2DList[i].y);
        }

        Triangulator2 tr = new Triangulator2(vertices2D);
        int[] indices = tr.Triangulate();

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        verticesTopoJson = vertices;

        triangles = indices;

    }
}
