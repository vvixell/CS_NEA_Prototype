using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DeleteRandomNodes
{
    public static Vector2[] GetNewPoints(int seed, int NumberOfTotalPoints, Vector2[] points)
    {
        List<Vector2> Points = points.ToList();
        List<Vector2> NewPoints = new List<Vector2>();

        System.Random rng = new System.Random(seed);
        for (int i = 0; i < NumberOfTotalPoints; i++)
        {
            int index = rng.Next(0, points.Length);
            NewPoints.Add(points[i]);
            Points.RemoveAt(i);
        }

        return NewPoints.ToArray();
    }
}
