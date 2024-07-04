using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomEdgeWeights
{
    public static int[] GetEdgeWeights(int seed, int EdgesCount)
    {
        System.Random rng = new System.Random(seed);

        int[] weights = new int[EdgesCount];

        for (int i = 0; i < EdgesCount; i++)
            weights[i] = rng.Next(1, 100);

        return weights;
    }
}
