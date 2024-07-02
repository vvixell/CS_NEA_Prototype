using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DelaunayTriangulation
{
    public static int[,] Triangulate(Vector2[] points)
    {
        List<Triangle> triangles = new List<Triangle>();

        Triangle superTriangle = FindSuperTriagle(points);
        triangles.Add(superTriangle);

        int PointIndex = -1;
        foreach (Vector2 point in points)
        {
            PointIndex++;
            List<Triangle> encompassingTriangles = new List<Triangle>();
            foreach (Triangle triangle in triangles)
            {
                float distance = Mathf.Sqrt(Mathf.Pow(point.x - triangle.Circumcenter.x, 2) + Mathf.Pow(point.y - triangle.Circumcenter.y, 2));
                if (distance < triangle.Radius)
                    encompassingTriangles.Add(triangle);
            }

            List<Edge> newPolygon = new List<Edge>();

            foreach (Triangle triangle in encompassingTriangles)
            {
                Edge[] triangleEdges = { triangle.A, triangle.B, triangle.C };

                foreach (Edge edge in triangleEdges)
                {
                    bool EdgeIsShared = false;
                    foreach (Triangle otherTriangle in encompassingTriangles)
                    {
                        if (otherTriangle.Equals(triangle)) continue;

                        Edge[] otherTriangleEdges = { otherTriangle.A, otherTriangle.B, otherTriangle.C };

                        foreach (Edge otherTriangleEdge in otherTriangleEdges)
                        {
                            if (otherTriangleEdge.Equals(edge))
                            {
                                EdgeIsShared = true;
                                break;
                            }
                        }
                        if (EdgeIsShared) break;
                    }
                    if (!EdgeIsShared) newPolygon.Add(edge); 
                }
            }
            Debug.Log(newPolygon.Count);
            foreach (Triangle triangle in encompassingTriangles)
                triangles.Remove(triangle);

            foreach (Edge edge in newPolygon)
            {
                Edge A = new Edge(edge.B, point, new int[] { edge.Indexes[1], PointIndex });
                Edge B = new Edge(point, edge.A, new int[] { PointIndex, edge.Indexes[0] });
                
                triangles.Add(new Triangle(A, B, edge));
            }
            Debug.Log(triangles.Count);
        }

        Edge[] superTriangleEdges = { superTriangle.A, superTriangle.B, superTriangle.C };
        foreach (Triangle triangle in triangles.ToArray())
        {
            Edge[] triangleEdges = { triangle.A, triangle.B, triangle.C };
            foreach (Edge edge in triangleEdges)
            {
                if(edge.Indexes.ToList().Contains(-1)) triangles.Remove(triangle);
            }
        }

        List<Edge> edges = new List<Edge>();

        foreach (Triangle triangle in triangles)
        {
            Edge[] triangleEdges = { triangle.A, triangle.B, triangle.C };

            foreach (Edge triangleEdge in triangleEdges)
            {
                bool ContainsEdge = false;
                foreach (Edge edge in edges)
                {
                    if (edge.Equals(triangleEdge))
                        ContainsEdge = true;
                }
                if (ContainsEdge == false) edges.Add(triangleEdge);
            }
            
        }

        int[,] EdgesIndex = new int[edges.Count, 2];

        for (int i = 0; i < edges.Count; i++)
        {
            Edge currentEdge = edges[i];

            EdgesIndex[i,0] = currentEdge.Indexes[0];
            EdgesIndex[i,1] = currentEdge.Indexes[1];
        }
        return EdgesIndex;
    }

    private static Triangle FindSuperTriagle(Vector2[] points)
    {
        Vector2 triangleCenter = Vector2.zero;
        float triangleRadius = float.MinValue;

        foreach (Vector2 vector in points)
            triangleCenter += vector;
        triangleCenter /= points.Length;

        foreach (Vector2 vector in points)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(vector.x - triangleCenter.x, 2) + Mathf.Pow(vector.y - triangleCenter.y, 2));
            if (distance > triangleRadius) triangleRadius = distance;
        }

        triangleRadius++;

        Vector2 Top = new Vector2(triangleCenter.x, triangleCenter.y + triangleRadius / Mathf.Cos(60 * Mathf.Deg2Rad));
        Vector2 BottomLeft = new Vector2(triangleCenter.x - triangleRadius * Mathf.Tan(60 * Mathf.Deg2Rad), triangleCenter.y - triangleRadius);
        Vector2 BottomRight = new Vector2(triangleCenter.x + triangleRadius * Mathf.Tan(60 * Mathf.Deg2Rad), triangleCenter.y - triangleRadius);

        Edge A = new Edge(Top, BottomLeft, new int[]{ -1, -1 });
        Edge B = new Edge(BottomLeft, BottomRight, new int[] { -1, -1 });
        Edge C = new Edge(BottomRight, Top, new int[] { -1, -1 });

        Triangle superTriangle = new Triangle(A, B, C);

        return superTriangle;
    }

    struct Edge
    {
        public Vector2 A, B;
        public int[] Indexes;
        public Vector2 Midpoint;
        public float Gradient;

        public Edge(Vector2 _A, Vector2 _B, int[] _Indexes)
        {
            A = _A;
            B = _B;
            Indexes = _Indexes;

            Midpoint = new Vector2((A.x + B.x) / 2f, (A.y + B.y) / 2f);
            Gradient = (A.y - B.y) / (A.x - B.x);
        }
        public bool Equals(Edge edge)
        {
            bool IsEqual = (A == edge.A && B == edge.B) || (A == edge.B && B == edge.A);
            return IsEqual;
        }
    }

    struct Triangle
    {
        public Edge A, B, C;
        public Vector2 Circumcenter;

        public float Radius;

        public Triangle(Edge _A, Edge _B, Edge _C)
        {
            A = _A;
            B = _B;
            C = _C;

            Circumcenter = new Vector2(0, 0);
            Radius = 0;

            Circumcenter = CalculateCircumCenter();

            Radius = CalculateRadius();
        }

        Vector2 CalculateCircumCenter()
        {
            float NegativeGradientA = -A.Gradient;
            float NegativeGradientB = -B.Gradient;

            float interceptPerpenidularA = A.Midpoint.y - NegativeGradientA * A.Midpoint.x;
            float interceptPerpenidularB = B.Midpoint.y - NegativeGradientB * B.Midpoint.x;

            return new Vector2((interceptPerpenidularB - interceptPerpenidularA) / (NegativeGradientA - NegativeGradientB),
                (NegativeGradientB * interceptPerpenidularA - NegativeGradientA * interceptPerpenidularB) / (NegativeGradientA - NegativeGradientB)
                );
        }

        float CalculateRadius()
        {
            return Mathf.Sqrt(Mathf.Pow(A.A.x - Circumcenter.x, 2) + Mathf.Pow(A.A.y - Circumcenter.y, 2));
        }
    }

}

