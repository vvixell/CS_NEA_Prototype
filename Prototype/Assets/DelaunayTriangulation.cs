using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DelaunayTriangulation
{
    public static int[][] Triangulate(Vector2[] points)
    {
        List<Triangle> triangles = new List<Triangle>();

        Triangle superTriangle = FindSuperTriagle(points);
        triangles.Add(superTriangle);

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 point = points[i];

            List<Triangle> encompassingTriangles = new List<Triangle>();

            foreach (Triangle triangle in triangles)
            {
                float distance = Mathf.Sqrt(Mathf.Pow(point.x - triangle.Circumcenter.x, 2) + Mathf.Pow(point.y - triangle.Circumcenter.y, 2));
                if (distance < triangle.Radius)
                    encompassingTriangles.Add(triangle);
            }

            
            List<Vector2[]> newPolygon = new List<Vector2[]>();

            foreach (Triangle triangle in encompassingTriangles)
            {
                Vector2[][] trianglesEdges = new Vector2[][] {
                    new Vector2[] { triangle.Vertices[0], triangle.Vertices[1] },
                    new Vector2[] { triangle.Vertices[1], triangle.Vertices[2] },
                    new Vector2[] { triangle.Vertices[2], triangle.Vertices[0] }
                };

                foreach (Vector2[] edge in trianglesEdges)
                {
                    bool EdgeIsShared = false;

                    foreach (Triangle otherTriangle in encompassingTriangles)
                    {
                        if (otherTriangle.Equals(triangle)) continue;

                        Vector2[][] otherTriangleEdges = new Vector2[][] {
                            new Vector2[] { otherTriangle.Vertices[0], otherTriangle.Vertices[1] },
                            new Vector2[] { otherTriangle.Vertices[1], otherTriangle.Vertices[2] },
                            new Vector2[] { otherTriangle.Vertices[2], otherTriangle.Vertices[0] }
                        };

                        foreach (Vector2[] otherTriangleEdge in otherTriangleEdges)
                        {
                            if ((edge[0] == otherTriangleEdge[0] && edge[1] == otherTriangleEdge[1]) || (edge[0] == otherTriangleEdge[1] && edge[1] == otherTriangleEdge[0]))
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

            foreach (Triangle triangle in encompassingTriangles)
                triangles.Remove(triangle);

            foreach (Vector2[] edge in newPolygon)
                triangles.Add(new Triangle(edge[0], edge[1], point));
        }

        foreach (Triangle triangle in triangles.ToArray())
            if (triangle.SharesVertex(superTriangle)) triangles.Remove(triangle);

        List<int[]> edges = new List<int[]>();
        foreach (Triangle triangle in triangles)
        {
            int indexA = points.ToList().IndexOf(triangle.Vertices[0]);
            int indexB = points.ToList().IndexOf(triangle.Vertices[1]);
            int indexC = points.ToList().IndexOf(triangle.Vertices[2]);

            int[][] triangleEdges = new int[][]
            {
                new int[] { indexA, indexB },
                new int[] { indexB, indexC },
                new int[] { indexC, indexA }
            };
            foreach (int[] triangleEdge in triangleEdges)
            {
                bool ContainsEdge = false;

                foreach (int[] edge in edges)
                {
                    if (edge[0] == triangleEdge[0] && edge[1] == triangleEdge[1] || edge[1] == triangleEdge[0] && edge[0] == edge[1])
                        ContainsEdge = true;
                }
                if (ContainsEdge == false) edges.Add(triangleEdge);
            }

        }
        return edges.ToArray();
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

        Triangle superTriangle = new Triangle(Top, BottomLeft, BottomRight);

        return superTriangle;
    }

    struct Triangle
    {
        public Vector2[] Vertices;
        public Vector2 Circumcenter;

        public float Radius;

        public Triangle(Vector2 A, Vector2 B, Vector2 C)
        {
            Vertices = new Vector2[] { A, B, C };

            Circumcenter = new Vector2(0, 0);
            Radius = 0;

            Circumcenter = CalculateCircumCenter();

            Radius = CalculateRadius();
        }

        Vector2 CalculateCircumCenter()
        {
            Vector2 A = Vertices[0];
            Vector2 B = Vertices[1];
            Vector2 C = Vertices[2];

            float D = 2 * (A.x * (B.y - C.y) + B.x * (C.y - A.y) + C.x * (A.y - B.y));

            float x = 1 / D * ((Mathf.Pow(A.x, 2) + Mathf.Pow(A.y, 2)) * (B.y - C.y) + (Mathf.Pow(B.x, 2) + Mathf.Pow(B.y, 2)) * (C.y - A.y) + (Mathf.Pow(C.x, 2) + Mathf.Pow(C.y, 2)) * (A.y - B.y));
            float y = 1 / D * ((Mathf.Pow(A.x, 2) + Mathf.Pow(A.y, 2)) * (C.x - B.x) + (Mathf.Pow(B.x, 2) + Mathf.Pow(B.y, 2)) * (A.x - C.x) + (Mathf.Pow(C.x, 2) + Mathf.Pow(C.y, 2)) * (B.x - A.x));

            return new Vector2(x, y);
        }

        float CalculateRadius()
        {
            return Mathf.Sqrt(Mathf.Pow(Vertices[0].x - Circumcenter.x, 2) + Mathf.Pow(Vertices[0].y - Circumcenter.y, 2));
        }

        public bool SharesVertex(Triangle triangle)
        {
            int SharedVertices = 0;
            for (int i = 0; i < 3; i++)
            {
                if (triangle.Vertices.Contains(Vertices[i])) SharedVertices++;
            }
            return SharedVertices >= 1;
        }
    }

}

