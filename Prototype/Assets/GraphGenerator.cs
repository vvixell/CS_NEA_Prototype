using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphGenerator : MonoBehaviour
{
    [Header("Visuals")]
    public float PointScale;

    public SpriteRenderer rend;
    public LineRenderer line;


    public void Generate(int Seed, int GraphSize, int CavernCount, int DistanceBetweenCaverns, NoiseSettings settings,float Randomness, bool ExistingPathWeight, float ExistingPathWeightMultiplier, bool ShowPoints, bool ShowCaverns, bool ShowTriangulation, bool ShowEdgeWeights, bool ShowFinalCaveLines)
    {
        DestroyGraph();
        //new NoiseSettings(graphs, 1, 0.5f, 2f)
        Vector2[] Points = DiscSampling.GeneratePoints(Seed, settings, GraphSize, new int[] { DistanceBetweenCaverns / 2, DistanceBetweenCaverns });

        int[] Caverns = DeleteRandomNodes.GetNewPointsIndexes(Seed, CavernCount, Points);

        int[,] AdjacencyMatrix = DelaunayTriangulation.Triangulate(Points);

        RandomEdgeWeights.SetEdgeWeights(Seed, ref AdjacencyMatrix, Randomness);

        int MainCavern = 0;

        int[,] CaveAdjacencyMatrix = Djikstras.GetCavePathAdjacencyMatrix(Points, Caverns, AdjacencyMatrix, MainCavern, ExistingPathWeight, ExistingPathWeightMultiplier);

        if (ShowPoints)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                if (ShowCaverns && Caverns.Contains(i)) continue;

                GameObject point = new GameObject($"{i} , {Points[i]}");
                point.transform.parent = transform;
                point.transform.position = Points[i];
                point.transform.localScale = Vector2.one * PointScale;
                SpriteRenderer pointRend = point.AddComponent<SpriteRenderer>();
                pointRend.sprite = rend.sprite;
                pointRend.material = rend.material;
            }
        }

        if (ShowCaverns)
        {
            for (int i = 0; i < Caverns.Length; i++)
            {
                GameObject point = new GameObject($"{Caverns[i]} , {Points[Caverns[i]]}");
                point.transform.parent = transform;
                point.transform.position = Points[Caverns[i]];
                point.transform.localScale = Vector2.one * PointScale;
                SpriteRenderer pointRend = point.AddComponent<SpriteRenderer>();
                pointRend.color = Color.cyan;
                pointRend.sprite = rend.sprite;
                pointRend.material = rend.material;
            }
        }

        if (ShowTriangulation)
        {
            for (int x = 0; x < AdjacencyMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < AdjacencyMatrix.GetLength(1); y++)
                {
                    if (AdjacencyMatrix[x, y] == 0) continue;
                    if (CaveAdjacencyMatrix[x, y] != 0 && ShowFinalCaveLines) continue;

                    Vector2 A = Points[x];
                    Vector2 B = Points[y];
                    GameObject edge = new GameObject($"{x} - {y}");
                    edge.transform.parent = transform;

                    edge.transform.position = new Vector2((A.x + B.x) / 2f, (A.y + B.y) / 2f);

                    LineRenderer l = edge.AddComponent<LineRenderer>();


                    l.positionCount = 3;
                    l.material = line.material;
                    if (!ShowEdgeWeights)
                        l.startWidth = line.startWidth;
                    else
                    {
                        float Multiplier = ((AdjacencyMatrix[x, y] / (Mathf.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y)) * 100)) / 20) + 0.5f;
                        l.widthCurve = new AnimationCurve(new Keyframe(0, line.startWidth), new Keyframe(0.5f, line.startWidth * Multiplier), new Keyframe(1, line.startWidth));
                    }
                    l.SetPositions(new Vector3[] { A, edge.transform.position, B });
                }
            }
        }

        if (ShowFinalCaveLines)
        {
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

                        l.positionCount = 3;
                        l.material = line.material;
                        l.startColor = Color.cyan;
                        l.endColor = Color.cyan;
                        if (!ShowEdgeWeights)
                            l.startWidth = line.startWidth * 1.5f;
                        else
                        {
                            float Multiplier = ((AdjacencyMatrix[x, y] / (Mathf.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y)) * 100)) / 20) + 0.5f;
                            l.widthCurve = new AnimationCurve(new Keyframe(0,line.startWidth), new Keyframe(0.5f, line.startWidth * Multiplier), new Keyframe(1, line.startWidth));
                        }
                        l.SetPositions(new Vector3[] { A, edge.transform.position, B });
                    }
                }
            }
        }
    }

    public void DestroyGraph()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}

