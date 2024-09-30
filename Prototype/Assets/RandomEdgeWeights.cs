using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomEdgeWeights
{
    public static void SetEdgeWeights(int seed, ref int[,] AdjacencyMatrix)
    {
        System.Random rng = new System.Random(seed);
        for(int a = 0; a < AdjacencyMatrix.GetLength(0); a++)
        {
            for(int b = 0; b < AdjacencyMatrix.GetLength(1); b++)
            {
                int weight = rng.Next(1, 100);
                AdjacencyMatrix[a,b] *= weight;
                AdjacencyMatrix[b,a] *= weight;
            }
        }
        
    }
}
