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

    public static int[] GetNewPointsIndexes(int seed, int NumberOfTotalPoints, Vector2[] points)
    {
        int[] Indexes = new int[points.Length];
        for(int i = 0; i < points.Length; i++) Indexes[i] = i;
        
        List<int> Points = Indexes.ToList();
        List<int> NewPoints = new List<int>();

        System.Random rng = new System.Random(seed + 1);
        for (int i = 0; i < NumberOfTotalPoints; i++)
        {
            int index = rng.Next(0, Points.Count);
            NewPoints.Add(Points[index]);
            Points.RemoveAt(index);
        }

        return NewPoints.ToArray();
    }
}
