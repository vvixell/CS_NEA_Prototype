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

        int[][] EdgesIndex = new int[triangles.Count * 3][];

        int TriangleIndex = 0;
        for (int i = 0; i < triangles.Count; i+=3)
        {
            Triangle triangle = triangles[TriangleIndex];
            TriangleIndex++;
            EdgesIndex[i] = new int[] { points.ToList().IndexOf(triangle.Vertices[0]), points.ToList().IndexOf(triangle.Vertices[1]) };
            EdgesIndex[i + 1] = new int[] { points.ToList().IndexOf(triangle.Vertices[1]), points.ToList().IndexOf(triangle.Vertices[2]) };
            EdgesIndex[i + 2] = new int[] { points.ToList().IndexOf(triangle.Vertices[2]), points.ToList().IndexOf(triangle.Vertices[0]) };
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

            Vector2 midpointAB = new Vector2((A.x + B.x) / 2f, (A.y + B.y) / 2f);
            Vector2 midpointBC = new Vector2((B.x + C.x) / 2f, (B.y + C.y) / 2f);

            float gradientAB = (A.y - B.y) / (A.x - B.x);
            float gradientBC = (B.y - C.y) / (B.x - C.x);

            float negativeGradientAB = -gradientAB;
            float negativeGradientBC = -gradientBC;

            float interceptPerpenidularAB = midpointAB.y - negativeGradientAB * midpointAB.x;
            float interceptPerpenidularBC = midpointBC.y - negativeGradientBC * midpointBC.x;

            return new Vector2((interceptPerpenidularBC - interceptPerpenidularAB) / (negativeGradientAB - negativeGradientBC),
                (negativeGradientBC * interceptPerpenidularAB - negativeGradientAB * interceptPerpenidularBC) / (negativeGradientAB - negativeGradientBC)
                );
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

        public bool SharesEdge(Triangle triangle)
        {
            int SharedVertices = 0;
            for (int i = 0; i < 3; i++)
            {
                if (triangle.Vertices.Contains(Vertices[i])) SharedVertices++;
            }
            return SharedVertices >= 2;
        }
    }

}

