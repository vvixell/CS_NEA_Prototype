using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    [Header("Visuals")]
    public float PointScale;

    [Header("Graph Settings")]
    public int Seed;
    public int GraphSize;
    public int CavernCount;
    public int DistanceBetweenCaverns;

    public SpriteRenderer rend;
    public LineRenderer line;

    void Start()
    {
        Vector2[] Points = DiscSampling.GeneratePoints(Seed, new NoiseSettings(1000, 1, 0.5f, 2f), GraphSize, new int[] { DistanceBetweenCaverns / 2, DistanceBetweenCaverns });

        int[] Caverns = DeleteRandomNodes.GetNewPointsIndexes(Seed, CavernCount, Points);

        int[,] AdjacencyMatrix = DelaunayTriangulation.Triangulate(Caverns);
        
        RandomEdgeWeights.SetEdgeWeights(Seed, ref AdjacencyMatrix);

        int MainCavern = 0;

        int[,] CaveAdjacencyMatrix = GetCavePathAdjacencyMatrix(Caverns, AdjacencyMatrix, MainCavern);

        for (int i = 0; i < Points.Length; i++)
        {
            GameObject point = new GameObject($"{i} , {Points[i]}");
            point.transform.position = Points[i];
            point.transform.localScale = Vector2.one * PointScale;
            SpriteRenderer pointRend = point.AddComponent<SpriteRenderer>();
            pointRend.sprite = rend.sprite;
            pointRend.material = rend.material;
        }

        for (int i = 0; i < Edges.Length; i++)
        {
            int[] Vertices = Edges[i];
            Vector2 A = Points[Vertices[0]]; 
            Vector2 B = Points[Vertices[1]];

            GameObject edge = new GameObject($"{Vertices[0]} - {Vertices[1]}");

            edge.transform.position = new Vector2((A.x + B.x) / 2f, (A.y + B.y) / 2f);

            LineRenderer l = edge.AddComponent<LineRenderer>();

            l.positionCount = 2;
            l.material = line.material;
            l.startWidth = line.startWidth;

            l.SetPositions(new Vector3[] { A, B });
        }
    }

   
}

