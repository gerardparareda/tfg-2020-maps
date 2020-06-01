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
        //mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;

        topoReader = new TopoJsonReader();
        topoReader.ParseTopoJSON(path);

        pos = new Vector2(0, 0);

        //SetGameObjectScale();

        //verticesTopoJson = topoReader.

        //CreateShape();
        CreateAllGameObjects();

        //CreateShapeFromTopoJSON();
        //UpdateMesh();
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
                CreateShapeFromTopoJSON(i);

                newNode.GetComponent<Node>().SetNewVerticesFromTopoJSON(verticesTopoJson);
                newNode.GetComponent<Node>().SetNewTrianglesFromTopoJSON(triangles);
                newNode.GetComponent<Node>().UpdateMesh();
                newNode.name = "Node" + i;
                newNode.transform.parent = this.transform;

                nodes.Add(newNode);
            } else 
            {
                //Todo MultiPolygon
            }
            
        }
    }

    void CreateShapeFromTopoJSON(int feature)
    {
        topoJsonWrapper topoData = topoReader.RetrieveJsonData();
        //int todoArc = topoData.objects.collection.geometries[0].arcs[0];

        List<Vector2> vertices2DList = new List<Vector2>();
        //Vector2 toAdd = new Vector2(0, 0);

        if(feature == 1)
        {
            //things
        }


        for (int arcFeature = 0; arcFeature < topoData.objects.collection.geometries[feature].arcs[0].Count; arcFeature++)
        {
            if(topoData.objects.collection.geometries[feature].arcs[0][arcFeature] < 0)
            {

                //TODO
                if (arcFeature == 0)
                {
                    for (int arcVertDisplace = 0; arcVertDisplace < topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])].Count - 1; arcVertDisplace++)
                    {

                        if (arcFeature > 0 && arcVertDisplace == 0)
                        {
                            continue;
                        }

                        if (arcFeature == 0 && arcVertDisplace == 0)
                        {
                            //vertices2DList.Add(new Vector2(pos.x, pos.y));
                            //continue;
                            pos.x = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];

                        }
                        else
                        {
                            pos.x += topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y += topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];

                        }

                        vertices2DList.Add(new Vector2(pos.x, pos.y));
                    }
                    vertices2DList.Reverse();
                    pos.x = vertices2DList[vertices2DList.Count-1].x;
                    pos.y = vertices2DList[vertices2DList.Count-1].y;
                }
                else
                {
                    int tmpArcFeat = topoData.objects.collection.geometries[feature].arcs[0][arcFeature];
                    //Reversed
                    for (int arcVertDisplace = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])].Count - 1; arcVertDisplace > 0; arcVertDisplace--)
                    {

                        //Saltar-se la primera delta dels arcFeature de la primera feature que no sigui la del principi
                        /*if (arcFeature > 0 && arcVertDisplace == topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])].Count - 1)
                        {
                            continue;
                        }*/

                        //Saltar-se la primera delta dels arcFeature quan ja no és la primera feature
                        if (arcFeature == 0 && arcVertDisplace == topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])].Count - 1)
                        {
                            //vertices2DList.Add(new Vector2(pos.x, pos.y));
                            //continue;
                            pos.x = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y = topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];
                        }
                        else
                        {
                            pos.x -= topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].x * topoData.transform.scale[0];
                            pos.y -= topoData.arcs[-1 * (1 + topoData.objects.collection.geometries[feature].arcs[0][arcFeature])][arcVertDisplace].y * topoData.transform.scale[1];

                        }

                        vertices2DList.Add(new Vector2(pos.x, pos.y));

                    }
                }

                
            } else
            {
                int tmpArcFeat = topoData.objects.collection.geometries[feature].arcs[0][arcFeature];
                for (int arcVertDisplace = 0; arcVertDisplace < topoData.arcs[topoData.objects.collection.geometries[feature].arcs[0][arcFeature]].Count; arcVertDisplace++)
                {

                    if (arcFeature > 0 && arcVertDisplace == 0)
                    {
                        continue;
                    }

                    if (arcFeature == 0 && arcVertDisplace == 0)
                    {
                        //vertices2DList.Add(new Vector2(pos.x, pos.y));
                        //continue;
                        pos.x = topoData.arcs[topoData.objects.collection.geometries[feature].arcs[0][arcFeature]][arcVertDisplace].x * topoData.transform.scale[0];
                        pos.y = topoData.arcs[topoData.objects.collection.geometries[feature].arcs[0][arcFeature]][arcVertDisplace].y * topoData.transform.scale[1];

                    } else
                    {
                        pos.x += topoData.arcs[topoData.objects.collection.geometries[feature].arcs[0][arcFeature]][arcVertDisplace].x * topoData.transform.scale[0];
                        pos.y += topoData.arcs[topoData.objects.collection.geometries[feature].arcs[0][arcFeature]][arcVertDisplace].y * topoData.transform.scale[1];
                        
                    }

                   vertices2DList.Add(new Vector2(pos.x, pos.y));
                }
            }
        }

        //verticesTopoJson = new Vector3[sizeVertices]; //Change arcs to those of geometry
        Vector2[] vertices2D = new Vector2[vertices2DList.Count];
        Vector3[] vertices = new Vector3[vertices2D.Length];

        for(int i = 0; i < vertices2DList.Count; i++)
        {
            vertices2D[i] = new Vector2(vertices2DList[i].x, vertices2DList[i].y);
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

}
