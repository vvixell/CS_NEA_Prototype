using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


    public void Generate(int Seed, int GraphSize, int CavernCount, int DistanceBetweenCaverns)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Vector2[] Points = DiscSampling.GeneratePoints(Seed, new NoiseSettings(1000, 1, 0.5f, 2f), GraphSize, new int[] { DistanceBetweenCaverns / 2, DistanceBetweenCaverns });

        int[] Caverns = DeleteRandomNodes.GetNewPointsIndexes(Seed, CavernCount, Points);

        int[,] AdjacencyMatrix = DelaunayTriangulation.Triangulate(Points);

        RandomEdgeWeights.SetEdgeWeights(Seed, ref AdjacencyMatrix);

        int MainCavern = 0;

        int[,] CaveAdjacencyMatrix = Djikstras.GetCavePathAdjacencyMatrix(Points, Caverns, AdjacencyMatrix, MainCavern);


        for (int x = 0; x < CaveAdjacencyMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < CaveAdjacencyMatrix.GetLength(1); y++)
            {
                if (CaveAdjacencyMatrix[x, y] != 0)
                {
                    Vector2 A = Points[x];
                    Vector2 B = Points[y];

                    GameObject edge = new GameObject($"{x} - {y}");
                    edge.transform.parent = transform;

                    edge.transform.position = new Vector2((A.x + B.x) / 2f, (A.y + B.y) / 2f);

                    LineRenderer l = edge.AddComponent<LineRenderer>();

                    l.positionCount = 2;
                    l.material = line.material;
                    l.startWidth = line.startWidth;

                    l.SetPositions(new Vector3[] { A, B });
                }
            }
        }

        for (int i = 0; i < Points.Length; i++)
        {
            GameObject point = new GameObject($"{i} , {Points[i]}");
            point.transform.parent = transform;
            point.transform.position = Points[i];
            point.transform.localScale = Vector2.one * PointScale;
            SpriteRenderer pointRend = point.AddComponent<SpriteRenderer>();
            if (Caverns.Contains(i)) pointRend.color = Color.red;
            pointRend.sprite = rend.sprite;
            pointRend.material = rend.material;
        }
    }
}

