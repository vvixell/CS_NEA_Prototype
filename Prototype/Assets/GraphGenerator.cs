using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public float PointScale;

    public int MapSize;

    public int Seed;

    public int CloseDBC; //DBC - Distance between caverns
    public int FarDBC;

    public int CandidatePointAmount;

    public NoiseSettings settings;

    public SpriteRenderer rend;
    public LineRenderer line;

    public Vector2[] Points;

    void Start()
    {
        
       // Vector2[] Points = DiscSampling.GeneratePoints(Seed, settings, MapSize, new int[] { CloseDBC, FarDBC });
        int[,] Edges = DelaunayTriangulation.Triangulate(Points);

        
        for (int i = 0; i < Points.Length; i++)
        {
            GameObject point = new GameObject($"{i} , {Points[i]}");
            point.transform.position = Points[i];
            point.transform.localScale = Vector2.one * PointScale;
            SpriteRenderer pointRend = point.AddComponent<SpriteRenderer>();
            pointRend.sprite = rend.sprite;
            pointRend.material = rend.material;
        }

        for (int i = 0; i < Edges.GetLength(0); i++)
        {
            Vector2 A = Points[Edges[i, 0]];
            Vector2 B = Points[Edges[i, 1]];
            GameObject edge = new GameObject($"{Edges[i, 0]} - {Edges[i, 1]}");
            edge.transform.position = new Vector2((A.x + B.x) / 2f, (A.y + B.y) / 2f);
            LineRenderer lineRenderer = edge.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, A);
            lineRenderer.SetPosition(1, B);
            lineRenderer.material = line.material;
            lineRenderer.startWidth = line.startWidth;
        }
    }

   
}

