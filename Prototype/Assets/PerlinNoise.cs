using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PerlinNoise
{
    public static float[,] CreateNoiseMap(int Seed, Vector2Int Size, NoiseSettings settings)
    {
        float[,] noiseMap = new float[Size.x, Size.y];

        System.Random rng = new System.Random(Seed);

        Vector2[] offsets = new Vector2[settings.Octaves];
        for (int i = 0; i < settings.Octaves; i++)
            offsets[i] = new Vector2(
                rng.Next(-99999, 99999),
                rng.Next(-99999, 99999)
                );

        float HalfSizeX = Size.x / 2; int HalfSizeY = Size.y / 2;

        float highestValue = float.MinValue; float lowestValue = float.MaxValue;
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                float frequency = 1;
                float amplitude = 1;
                float height = 0;
                
                for (int i = 0; i < settings.Octaves; i++)
                {
                    float sampleX = ((x - HalfSizeX) / settings.Scale * frequency) + offsets[i].x;
                    float sampleY = ((y - HalfSizeY) / settings.Scale * frequency) + offsets[i].y;

                    height += Sample(sampleX, sampleY) * amplitude;

                    amplitude *= settings.Persistence;
                    frequency *= settings.Lacunarity;
                }

                noiseMap[x, y] = height;
                if (height < lowestValue) lowestValue = height;
                else if (height > highestValue) highestValue = height;
            }
        }

        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                float height = noiseMap[x, y];
                noiseMap[x, y] = Mathf.InverseLerp(lowestValue, highestValue, height);
            }
        }
        return noiseMap;
    }

    public static float PointValue01(int Seed, Vector2 position, NoiseSettings settings)
    {
        System.Random rng = new System.Random(Seed);

        Vector2[] offsets = new Vector2[settings.Octaves];
        for (int i = 0; i < settings.Octaves; i++)
            offsets[i] = new Vector2(
                rng.Next(-99999, 99999),
                rng.Next(-99999, 99999)
                );

        float frequency = 1;
        float amplitude = 1;
        float height = 0;

        for (int i = 0; i < settings.Octaves; i++)
        {
            float sampleX = (position.x / settings.Scale * frequency) + offsets[i].x;
            float sampleY = (position.y / settings.Scale * frequency) + offsets[i].y;

            height += Sample(sampleX, sampleY) * amplitude;

            amplitude *= settings.Persistence;
            frequency *= settings.Lacunarity;
        }

        if (height < 0) return 0;
        return 1;
    }

    private static float Sample(float x, float y) 
    {
        int GridX = Mathf.FloorToInt(x);
        int GridY = Mathf.FloorToInt(y);

        float LocalX = x - GridX;
        float LocalY = y - GridY;

        

        Vector2[] GridCoordinates = new Vector2[]
        {
            new Vector2(GridX, GridY + 1), //Top Left
            new Vector2(GridX + 1, GridY + 1), //Top Right
            new Vector2(GridX + 1, GridY), //Bottom Right
            new Vector2(GridX, GridY), //Bottom Left 
        };

        float DotTopLeft = DotProduct(new Vector2(x - GridCoordinates[0].x, y - GridCoordinates[0].y), GetDirectionVector(GridCoordinates[0]));
        float DotTopRight = DotProduct(new Vector2(x - GridCoordinates[1].x, y - GridCoordinates[1].y), GetDirectionVector(GridCoordinates[1]));
        float DotBottomRight = DotProduct(new Vector2(x - GridCoordinates[2].x, y - GridCoordinates[2].y), GetDirectionVector(GridCoordinates[2]));
        float DotBottomLeft = DotProduct(new Vector2(x - GridCoordinates[3].x, y - GridCoordinates[3].y), GetDirectionVector(GridCoordinates[3]));

        return smoothstep(LocalY, smoothstep(LocalX, DotBottomLeft, DotBottomRight), smoothstep(LocalX, DotTopLeft, DotTopRight));  
    }

    private static Vector2 GetDirectionVector(Vector2 key)
    {
        int DistributedNum = (((int)key.x * 36520361) + ((int)key.y * 46211677) % 78842663);
        DistributedNum ^= DistributedNum << 13;
        DistributedNum ^= DistributedNum >> 17;
        DistributedNum ^= DistributedNum << 5;

        float angle = (DistributedNum % 360) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }

    private static float DotProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static float interpolate(float t, float a, float b)
    {
        return  t * (b - a) + a;
    }

    public static float smoothstep(float t, float a, float b)
    {
        return (b - a) * (3 - t * 2) * t * t + a;
    }
}

[System.Serializable]
public class NoiseSettings
{
    public float Scale;
    public int Octaves;
    public float Persistence;
    public float Lacunarity;
}