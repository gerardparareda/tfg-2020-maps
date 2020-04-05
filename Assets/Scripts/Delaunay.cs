using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaunay : MonoBehaviour
{
    const double EPSILON = 0.000001;

    public static int[] Triangulate(Vector3[] points)
    {
        int lengthTriangles = points.Length * 3;
        int[] triangulated;
        List<Triangle> trianglesWithSuper = new List<Triangle>();

        //Calculate super triangle
        trianglesWithSuper.Add(CalculateSuperTriangle(points));

        Triangle superTriangle = CalculateSuperTriangle(points);

        foreach (Vector3 newPoint in points)
        {
            AddNewVertex(newPoint, trianglesWithSuper);
        }

        //Delete super triangle

        //Remove all triangles whose edges are created with the super triangle verices
        //TODO
        RemoveSuperTriangle(trianglesWithSuper, superTriangle);

        //Delete the super triangle
        //trianglesWithSuper.RemoveAt(0);

        triangulated = new int[trianglesWithSuper.Count * 3];

        for(int i = 0; i < trianglesWithSuper.Count; i++)
        {
            for(int j = 0; j < points.Length; j++)
            {
                if(trianglesWithSuper[i].v1 == points[j])
                {
                    triangulated[i * 3] = j;
                }

                if (trianglesWithSuper[i].v2 == points[j])
                {
                    triangulated[i * 3 + 1] = j;
                }

                if (trianglesWithSuper[i].v3 == points[j])
                {
                    triangulated[i * 3 + 2] = j;
                }
            }

        }

        return triangulated;
    }

    static void RemoveSuperTriangle(List<Triangle> trianglesWithSuper, Triangle superTriangle)
    {
        for(int i = 0; i < trianglesWithSuper.Count; i++)
        {
            if(
                trianglesWithSuper[i].v1 == superTriangle.v1 || 
                trianglesWithSuper[i].v1 == superTriangle.v2 ||
                trianglesWithSuper[i].v1 == superTriangle.v3 ||

                trianglesWithSuper[i].v2 == superTriangle.v1 ||
                trianglesWithSuper[i].v2 == superTriangle.v2 ||
                trianglesWithSuper[i].v2 == superTriangle.v3 ||

                trianglesWithSuper[i].v3 == superTriangle.v1 ||
                trianglesWithSuper[i].v3 == superTriangle.v2 ||
                trianglesWithSuper[i].v3 == superTriangle.v3
                )
            {
                trianglesWithSuper.RemoveAt(i);
                i--;
            }
        }
    }

    static void AddNewVertex(Vector3 newPoint, List<Triangle> TList)
    {
        List<Edge> edges_list = new List<Edge>();

        //Check if it's inside of a circumcercle of other triangle
        for (int ti = 0; ti < TList.Count; ti++)
        {
            if (TList[ti].CheckIsInsideCircumcircle(newPoint))
            {
                Triangle currentTriangle = TList[ti];

                //Create new Edges
                Edge edgeA = new Edge(currentTriangle.v1, currentTriangle.v2);
                Edge edgeB = new Edge(currentTriangle.v2, currentTriangle.v3);
                Edge edgeC = new Edge(currentTriangle.v3, currentTriangle.v1);

                edges_list.Add(edgeA);
                edges_list.Add(edgeB);
                edges_list.Add(edgeC);

                TList.RemoveAt(ti);
                ti--;
            }

        }

        //Lets erase the repeated edges
        for (int i = 0; i < edges_list.Count; i++)
        {
            for (int j = i + 1; j < edges_list.Count; j++)
            {
                if (edges_list[i].Compare(edges_list[j]))
                {
                    //Delete the the second edge
                    edges_list.RemoveAt(j);
                    edges_list.RemoveAt(i);
                    i--;
                }
            }
        }

        //Make new edges from non-repeated edges vertices to the new point
        for (int ei = 0; ei < edges_list.Count; ei++)
        {
            Triangle newTriangle = new Triangle(edges_list[ei].v1, edges_list[ei].v2, newPoint);

            TList.Add(newTriangle);
        }

        edges_list.Clear();


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
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;

        public Edge e1;
        public Edge e2;
        public Edge e3;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;

            this.e1 = new Edge(v1, v2);
            this.e2 = new Edge(v2, v3);
            this.e3 = new Edge(v3, v1);
        }

        public bool CheckIsInsideCircumcircle(Vector3 newVertex)
        {
            Vector3 A = v1;
            Vector3 B = v2;
            Vector3 C = v3;
            double m1, m2, mx1, mx2, my1, my2, xc, yc, r;
            double dx, dy, rsqr, drsqr;

            if (Mathf.Abs(A.y - B.y) < EPSILON && Mathf.Abs(B.y - C.y) < EPSILON)
                return (false);
            if (Mathf.Abs(B.y - A.y) < EPSILON)
            {
                m2 = -(C.x - B.x) / (C.y - B.y);
                mx2 = (B.x + C.x) / 2.0;
                my2 = (B.y + C.y) / 2.0;
                xc = (B.x + A.x) / 2.0;
                yc = m2 * (xc - mx2) + my2;
            }
            else if (Mathf.Abs(C.y - B.y) < EPSILON)
            {
                m1 = -(B.x - A.x) / (B.y - A.y);
                mx1 = (A.x + B.x) / 2.0;
                my1 = (A.y + B.y) / 2.0;
                xc = (C.x + B.x) / 2.0;
                yc = m1 * (xc - mx1) + my1;
            }
            else
            {
                m1 = -(B.x - A.x) / (B.y - A.y);
                m2 = -(C.x - B.x) / (C.y - B.y);
                mx1 = (A.x + B.x) / 2.0;
                mx2 = (B.x + C.x) / 2.0;
                my1 = (A.y + B.y) / 2.0;
                my2 = (B.y + C.y) / 2.0;
                xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                yc = m1 * (xc - mx1) + my1;
            }
            dx = B.x - xc;
            dy = B.y - yc;
            rsqr = dx * dx + dy * dy;
            dx = newVertex.x - xc;
            dy = newVertex.y - yc;
            drsqr = dx * dx + dy * dy;
            return ((drsqr <= rsqr) ? true : false);
        }
    }

    class Edge
    {
        public Vector3 v1;
        public Vector3 v2;

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

    }

}
