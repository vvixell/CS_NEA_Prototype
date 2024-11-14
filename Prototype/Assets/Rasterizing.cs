using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rasterizing
{
    public static int[,] RasterizeCaveMap(Vector2[] Points, int[] Caverns, int[,] AdjacencyMatrix)
    {

        return new int[1, 1];
    }

    private static void RasterizeCircle(Vector2 Position, int Radius, ref int[] Image)
    {
        int x = Mathf.RoundToInt(Position.x);
        int y = Mathf.RoundToInt(Position.y);
        
    }
}
