using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiscSampling
{
    public static Vector2[] GeneratePoints(int Seed, NoiseSettings settings, int MapSize, int[] Radii, int CandidatePointAmount = 10)
    {
        System.Random rng = new System.Random(Seed);

        float cellSize = Mathf.Sqrt(Mathf.Pow(Radii[0], 2) / 2f);
        int gridSize = Mathf.CeilToInt(MapSize / cellSize);
        List<Point>[,] Grid = new List<Point>[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
                Grid[x, y] = new List<Point>();

        List<Point> ActivePoints = new List<Point>();
        List<Point> InactivePoints = new List<Point>();

        Vector2 PointPosition = new Vector2(rng.Next(-MapSize / 2, MapSize / 2), rng.Next(-MapSize / 2, MapSize / 2));
        float PointRadius = PerlinNoise.PointValue01(Seed, PointPosition, settings) == 0 ? Radii[0] : Radii[1];
        Point StartPoint = new Point(PointPosition, PointRadius);
        ActivePoints.Add(StartPoint);

        UpdateGrid(StartPoint, ref Grid, Radii);

        while (ActivePoints.Count > 0)
        {
            Point currentPoint = ActivePoints[0];
            List<Point> candidatePoints = new List<Point>();

            for (int i = 0; i < CandidatePointAmount; i++)
            {
                float RandomDirection = (float)(rng.NextDouble() * 2 * Mathf.PI);
                float DistanceFromPoint = ((float)rng.NextDouble() + 1) * currentPoint.Radius;

                Vector2 candidatePointPosition = new Vector2(Mathf.Sin(RandomDirection), Mathf.Cos(RandomDirection)) * DistanceFromPoint + currentPoint.Position;
                float candidatePointRadius = PerlinNoise.PointValue01(Seed, candidatePointPosition, settings) == 0 ? Radii[0] : Radii[1];

                candidatePoints.Add(new Point(candidatePointPosition, candidatePointRadius));
            }

            foreach (Point point in candidatePoints.ToArray())
            {
                if (point.Position.x < -MapSize / 2f || point.Position.x > MapSize / 2f || point.Position.y < -MapSize / 2f || point.Position.y > MapSize / 2f)
                {
                    candidatePoints.Remove(point);
                    continue;
                }

                if (!IsPositionValid(point, Grid, Radii))
                {
                    candidatePoints.Remove(point);
                    continue;
                }

                ActivePoints.Add(point);
                UpdateGrid(point, ref Grid, Radii);
            }

            ActivePoints.Remove(currentPoint);
            InactivePoints.Add(currentPoint);
        }

        Vector2[] pointPoisitions = new Vector2[InactivePoints.Count];

        for (int i = 0; i < pointPoisitions.Length; i++)
            pointPoisitions[i] = InactivePoints[i].Position;

        return pointPoisitions;
    }

    private static void UpdateGrid(Point point, ref List<Point>[,] Grid, int[] Radii)
    {
        int gridX = Mathf.FloorToInt(point.Position.x / Radii[0]) + Grid.GetLength(0) / 2;
        int gridY = Mathf.FloorToInt(point.Position.y / Radii[0]) + Grid.GetLength(1) / 2;

        int gridRadius = Mathf.CeilToInt(point.Radius / Radii[0]);

        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                Vector2Int pos = new Vector2Int(gridX + x, gridY + y);
                if (pos.x < 0 || pos.x >= Grid.GetLength(0) || pos.y < 0 || pos.y >= Grid.GetLength(1)) continue;

                Grid[pos.x, pos.y].Add(point);
            }
        }
    }

    public static bool IsPositionValid(Point point, List<Point>[,] Grid, int[] Radii)
    {
        int gridX = Mathf.FloorToInt(point.Position.x / Radii[0]) + Grid.GetLength(0) / 2;
        int gridY = Mathf.FloorToInt(point.Position.y / Radii[0]) + Grid.GetLength(1) / 2;

        foreach (Point p in Grid[gridX, gridY])
        {
            float distance = Mathf.Sqrt(Mathf.Pow(point.Position.x - p.Position.x, 2) + Mathf.Pow(point.Position.y - p.Position.y, 2));

            if (distance < p.Radius) return false;
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