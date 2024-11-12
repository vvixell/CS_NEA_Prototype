using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DelaunayTriangulation
{
    public static int[,] Triangulate(Vector2[] positions)
    {
        List<Triangle> triangles = new List<Triangle>();
        List<Point> points = new List<Point> ();

        Triangle superTriangle = FindSuperTriagle(positions, ref points);
        triangles.Add(superTriangle);

        for (int i = 0; i < positions.Length; i++)
        {
            Point point = new Point(positions[i], i);
            points.Add(point);

            List<Triangle> encompassingTriangles = new List<Triangle>();

            foreach (Triangle triangle in triangles)
            {
                float distance = Mathf.Sqrt(Mathf.Pow(point.Position.x - triangle.Circumcenter.x, 2) + Mathf.Pow(point.Position.y - triangle.Circumcenter.y, 2));
                if (distance < triangle.Radius)
                    encompassingTriangles.Add(triangle);
            }

            
            List<Point[]> newPolygon = new List<Point[]>();

            foreach (Triangle triangle in encompassingTriangles)
            {
                Point[][] trianglesEdges = new Point[][] {
                    new Point[] { triangle.Vertices[0], triangle.Vertices[1] },
                    new Point[] { triangle.Vertices[1], triangle.Vertices[2] },
                    new Point[] { triangle.Vertices[2], triangle.Vertices[0] }
                };

                foreach (Point[] edge in trianglesEdges)
                {
                    bool EdgeIsShared = false;

                    foreach (Triangle otherTriangle in encompassingTriangles)
                    {
                        if (otherTriangle.Equals(triangle)) continue;

                        Point[][] otherTriangleEdges = new Point[][] {
                            new Point[] { otherTriangle.Vertices[0], otherTriangle.Vertices[1] },
                            new Point[] { otherTriangle.Vertices[1], otherTriangle.Vertices[2] },
                            new Point[] { otherTriangle.Vertices[2], otherTriangle.Vertices[0] }
                        };

                        foreach (Point[] otherTriangleEdge in otherTriangleEdges)
                        {
                            if ((edge[0].Equals(otherTriangleEdge[0]) && edge[1].Equals(otherTriangleEdge[1])) || (edge[0].Equals(otherTriangleEdge[1]) && edge[1].Equals(otherTriangleEdge[0])))
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

            foreach (Point[] edge in newPolygon)
                triangles.Add(new Triangle(edge[0], edge[1], point));
        }

        foreach (Triangle triangle in triangles.ToArray())
            if (triangle.SharesVertex(superTriangle)) triangles.Remove(triangle);

        int[,] AdjacencyMatrix = new int[positions.Length,positions.Length];
        foreach (Triangle triangle in triangles)
        {
            int A = triangle.Vertices[0].Index;
            int B = triangle.Vertices[1].Index;
            int C = triangle.Vertices[2].Index;

            Vector2 a = positions[A];
            Vector2 b = positions[B];
            Vector2 c = positions[C];
            
            int DistanceAB = (int)(Mathf.Sqrt((a.x - b.x )*(a.x - b.x) + (a.y - b.y )*(a.y - b.y))*100);
            int DistanceBC = (int)(Mathf.Sqrt((b.x - c.x )*(b.x - c.x) + (b.y - c.y )*(b.y - c.y))*100);
            int DistanceCA = (int)(Mathf.Sqrt((c.x - a.x )*(c.x - a.x) + (c.y - a.y )*(c.y - a.y))*100);
                                
            AdjacencyMatrix[A,B] = DistanceAB;
            AdjacencyMatrix[B,A] = DistanceAB;
            
            AdjacencyMatrix[B,C] = DistanceBC;
            AdjacencyMatrix[C,B] = DistanceBC;
            
            AdjacencyMatrix[C,A] = DistanceCA;
            AdjacencyMatrix[B,C] = DistanceCA;
        }

        return AdjacencyMatrix;
    }

    private static Triangle FindSuperTriagle(Vector2[] positions, ref List<Point> points)
    {
        Vector2 triangleCenter = Vector2.zero;
        float triangleRadius = float.MinValue;

        foreach (Vector2 vector in positions)
            triangleCenter += vector;
        triangleCenter /= positions.Length;

        foreach (Vector2 vector in positions)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(vector.x - triangleCenter.x, 2) + Mathf.Pow(vector.y - triangleCenter.y, 2));
            if (distance > triangleRadius) triangleRadius = distance;
        }

        triangleRadius++;

        Vector2 Top = new Vector2(triangleCenter.x, triangleCenter.y + triangleRadius / Mathf.Cos(60 * Mathf.Deg2Rad));
        Vector2 BottomLeft = new Vector2(triangleCenter.x - triangleRadius * Mathf.Tan(60 * Mathf.Deg2Rad), triangleCenter.y - triangleRadius);
        Vector2 BottomRight = new Vector2(triangleCenter.x + triangleRadius * Mathf.Tan(60 * Mathf.Deg2Rad), triangleCenter.y - triangleRadius);

        Point TopPoint = new Point(Top, -1);
        Point BottomLeftPoint = new Point(BottomLeft, -1);
        Point BottomRightPoint = new Point(BottomRight, -1);
        points.Add(TopPoint);
        points.Add(BottomLeftPoint);
        points.Add(BottomRightPoint);

        Triangle superTriangle = new Triangle(TopPoint, BottomLeftPoint, BottomRightPoint);

        return superTriangle;
    }

    struct Triangle
    {
        public Point[] Vertices;
        public Vector2 Circumcenter;

        public float Radius;

        public Triangle(Point A, Point B, Point C)
        {
            Vertices = new Point[] { A, B, C };

            Circumcenter = new Vector2(0, 0);
            Radius = 0;

            Circumcenter = CalculateCircumCenter();

            Radius = CalculateRadius();
        }

        Vector2 CalculateCircumCenter()
        {
            Vector2 A = Vertices[0].Position;
            Vector2 B = Vertices[1].Position;
            Vector2 C = Vertices[2].Position;

            float D = 2 * (A.x * (B.y - C.y) + B.x * (C.y - A.y) + C.x * (A.y - B.y));

            float x = 1 / D * ((Mathf.Pow(A.x, 2) + Mathf.Pow(A.y, 2)) * (B.y - C.y) + (Mathf.Pow(B.x, 2) + Mathf.Pow(B.y, 2)) * (C.y - A.y) + (Mathf.Pow(C.x, 2) + Mathf.Pow(C.y, 2)) * (A.y - B.y));
            float y = 1 / D * ((Mathf.Pow(A.x, 2) + Mathf.Pow(A.y, 2)) * (C.x - B.x) + (Mathf.Pow(B.x, 2) + Mathf.Pow(B.y, 2)) * (A.x - C.x) + (Mathf.Pow(C.x, 2) + Mathf.Pow(C.y, 2)) * (B.x - A.x));

            return new Vector2(x, y);
        }

        float CalculateRadius()
        {
            return Mathf.Sqrt(Mathf.Pow(Vertices[0].Position.x - Circumcenter.x, 2) + Mathf.Pow(Vertices[0].Position.y - Circumcenter.y, 2));
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

    struct Point
    {
        public Vector2 Position;
        public int Index;
        
        public Point(Vector2 Position, int Index)
        {
            this.Position = Position;
            this.Index = Index;
        } 
    }
}

