using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public int MapSize;

    public int Seed;

    public int CloseDBC; //DBC - Distance between caverns
    public int FarDBC;

    public int MaximumSamplingTries;

    public NoiseSettings settings;

    public SpriteRenderer rend; 

    void Start()
    {
        Vector2[] p = GeneratePoints();

        foreach (Vector2 a in p)
        {
            GameObject b = new GameObject($"{a}");
            SpriteRenderer r = b.AddComponent<SpriteRenderer>();
            b.transform.position = a;
            r.sprite = rend.sprite;
            r.material = rend.material;
            b.transform.localScale = transform.localScale;
        }
    }

    public Vector2[] GeneratePoints()
    {
        System.Random rng = new System.Random(Seed);

        float cellSize = Mathf.Sqrt(Mathf.Pow(CloseDBC, 2) / 2f);
        int gridSize = Mathf.CeilToInt(MapSize / cellSize);
        List<int>[,] Grid = new List<int>[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Grid[x, y] = new List<int>();
            }
        }

        List<Point> ActivePoints = new List<Point>();
        Vector2 PointPosition = new Vector2(rng.Next(-MapSize/2, MapSize/2), rng.Next(-MapSize/2, MapSize/2));
        float PointRadius = PerlinNoise.PointValue01(PointPosition, settings) == 0 ? CloseDBC : FarDBC;
        ActivePoints.Add(new Point(PointPosition, PointRadius));

        int lastPoint = 0;
        UpdateGrid(lastPoint, ActivePoints, ref Grid);

        for (int i = 0; i < MaximumSamplingTries; i++)
        {
            float DistanceFromPoint = ((float)rng.NextDouble() + 1) * ActivePoints[lastPoint].Radius;
            float RandomDirection = (float)(rng.NextDouble() * 2 * Mathf.PI);

            PointPosition = new Vector2(Mathf.Sin(RandomDirection), Mathf.Cos(RandomDirection)) * DistanceFromPoint + ActivePoints[lastPoint].Position;
            if (PointPosition.x < -MapSize / 2f || PointPosition.x > MapSize / 2f || PointPosition.y < -MapSize / 2f || PointPosition.y > MapSize / 2f) continue;

            PointRadius = PerlinNoise.PointValue01(PointPosition, settings) == 0 ? CloseDBC : FarDBC;

            Point point = new Point(PointPosition, PointRadius);
            
            if (!IsPositionValid(point, ActivePoints, Grid)) continue;

            ActivePoints.Add(point);
            lastPoint++;
            UpdateGrid(lastPoint, ActivePoints, ref Grid);
        }

        Vector2[] pointPoisitions = new Vector2[ActivePoints.Count];

        for (int i = 0; i < pointPoisitions.Length; i++)
            pointPoisitions[i] = ActivePoints[i].Position;

        return pointPoisitions;
    }

    private void UpdateGrid(int PointIndex, List<Point> ActivePoints, ref List<int>[,] Grid)
    {
        Point point = ActivePoints[PointIndex];
        int gridX = Mathf.FloorToInt(point.Position.x / CloseDBC) + Grid.GetLength(0) / 2;
        int gridY = Mathf.FloorToInt(point.Position.y / CloseDBC) + Grid.GetLength(1) / 2;

        int gridRadius = Mathf.CeilToInt(point.Radius / CloseDBC);

        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                Vector2Int pos = new Vector2Int(gridX + x, gridY + y);
                if (pos.x < 0 || pos.x >= Grid.GetLength(0) || pos.y < 0 || pos.y >= Grid.GetLength(1)) continue;

                Grid[pos.x, pos.y].Add(PointIndex);
            }
        }
    }

    public bool IsPositionValid(Point point, List<Point> ActivePoints, List<int>[,] Grid)
    {
        int gridX = Mathf.FloorToInt(point.Position.x / CloseDBC) + Grid.GetLength(0) / 2;
        int gridY = Mathf.FloorToInt(point.Position.y / CloseDBC) + Grid.GetLength(1) / 2;

        foreach (int i in Grid[gridX, gridY])
        {
            float distance = Mathf.Sqrt(Mathf.Pow(point.Position.x - ActivePoints[i].Position.x, 2) + Mathf.Pow(point.Position.y - ActivePoints[i].Position.y, 2));

            if (distance < ActivePoints[i].Radius) return false;
        }
        return true;
    }
}

public struct Point
{
    public Vector2 Position;
    public float Radius;

    public Point(Vector2 Position, float Radius)
    {
        this.Position = Position;
        this.Radius = Radius;
    }
}