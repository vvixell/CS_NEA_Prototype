using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashFunction : MonoBehaviour
{
    public int GridSize = 8;
    public LineRenderer template;

    void Start()
    {
        for (int x = -GridSize; x <= GridSize; x++)
        {
            for (int y = -GridSize; y <= GridSize; y++)
            {
                GameObject Line = new GameObject($"Vector [{x},{y}]");
                LineRenderer rend = Line.AddComponent<LineRenderer>();
                float Direction = (float)Hash(x, y);
                Vector2 vector = new Vector2(Mathf.Sin(Direction), Mathf.Cos(Direction));
                rend.positionCount = 2;
                rend.material = template.material;
                rend.startWidth = template.startWidth;
                rend.endWidth = template.endWidth;
                rend.SetPosition(0, new Vector2(x, y));
                rend.SetPosition(1, new Vector2(x, y) + (vector * 0.5f));
            }
        }
    }

    public double Hash(int x, int y)
    {
        /*
        uint w = 8 * sizeof(uint);
        uint s = w / 2; // rotation width
        uint a = (uint)x + 3332933, b = (uint)y - 7663951;
        a *= 3284157443; b ^= a << 4 * sizeof(uint) | a >> 8 * sizeof(uint) - 4 * sizeof(uint);
        b *= 1911520717; a ^= b << 4 * sizeof(uint) | b >> 8 * sizeof(uint) - 4 * sizeof(uint);
        a *= 2048419325;
        double random = a * (3.14159265 / ~(~0u >> 1)); // in [0, 2*Pi]
        return random;
        */

        int DistributedNum = ((x * 36520361) + (y * 46211677) % 78842663);
        DistributedNum ^= DistributedNum << 13;
        DistributedNum ^= DistributedNum >> 17;
        DistributedNum ^= DistributedNum << 5;

        return (DistributedNum % 360) * Mathf.Deg2Rad;
    }
}
