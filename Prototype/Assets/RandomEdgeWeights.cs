using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomEdgeWeights
{
    public static void SetEdgeWeights(int seed, ref int[,] AdjacencyMatrix, float Randomness)
    {
        if (Randomness <= 0) return;
        System.Random rng = new System.Random(seed);
        for(int a = 0; a < AdjacencyMatrix.GetLength(0); a++)
        {
            for(int b = 0; b < AdjacencyMatrix.GetLength(1); b++)
            {
                int weight = rng.Next(1, 10);
                AdjacencyMatrix[a,b] *= (int)(weight * Randomness) + 1;
                AdjacencyMatrix[b,a] *= (int)(weight * Randomness) + 1;
            }
        }
        
    }
}
