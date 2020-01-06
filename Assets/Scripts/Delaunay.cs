using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaunay : MonoBehaviour
{
    public static int[] Triangulate(Vector3[] points)
    {
        int lengthTriangles = (points.Length - 1) * 3;
        int[] triangulated = new int[lengthTriangles];
        List<Triangle> trianglesWithSuper = new List<Triangle>();

        //Calculate super triangle
        trianglesWithSuper.Add(CalculateSuperTriangle(points));

        foreach(Vector3 newPoint in points)
        {
            AddNewVertex(newPoint, trianglesWithSuper);
        }

        for (int i = 0; i < triangulated.Length; i++)
        {
            triangulated[i] = trianglesWithSuper[i];
        }

        return triangulated;
    }

    static void AddNewVertex(Vector3 newPoint, List<Triangle> TList)
    {
        //Find in which triangle it's in
        

        //Create three new triangles


        //Break old triangle


    }

    static Triangle CalculateSuperTriangle(Vector3[] points)
    { 

        float xmin = points[0].x;
        float xmax = xmin;
        float ymin = points[0].y;
        float ymax = ymin;

        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].x < xmin) xmin = points[i].x;
            if (points[i].x > xmax) xmax = points[i].x;
            if (points[i].y < ymin) ymin = points[i].y;
            if (points[i].y > ymax) ymax = points[i].y;
        }


        float dx = xmax - xmin;
        float dy = ymax - ymin;
        float dmax = (dx > dy) ? dx : dy;
        float xmid = (xmax + xmin) / 2;
        float ymid = (ymax + ymin) / 2;

        return new Triangle(
            new Vector3(xmid - 20 * dmax, ymid - dmax, 0),
            new Vector3(xmid + 20 * dmax, ymid - dmax, 0),
            new Vector3(xmid, ymid + 20 * dmax, 0)
            );
    }

    class Triangle
    {
        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;

            this.e1 = new Edge(v1, v2);
            this.e1 = new Edge(v2, v3);
            this.e1 = new Edge(v3, v1);
        }

        Vector3 v1;
        Vector3 v2;
        Vector3 v3;

        Edge e1;
        Edge e2;
        Edge e3;
    }

    class Edge
    {
        public Edge(Vector3 v1, Vector3 v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public bool Compare(Edge nE)
        {
            if(nE.v1 == this.v1 && nE.v2 == this.v2)
            {
                return true;
            }
            if(nE.v1 == this.v2 && nE.v2 == this.v1)
            {
                return true;
            }

            return false;
        }

        Vector3 v1;
        Vector3 v2;
    }

}
