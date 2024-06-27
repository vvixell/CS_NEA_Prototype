using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseTest : MonoBehaviour
{
    public int Seed;
    public Vector2Int Size;
    public float Scale;
    public int Octaves;
    public float Persistence;
    public float Lacunarity;

    public AnimationCurve CuttoffCurve;


    public RawImage img;
    public RawImage img2;

    void Start()
    {
        //CreateNoise();
    }

    /*public void CreateNoise()
    {
        Texture2D texture = new Texture2D(Size.x, Size.y);

        float[,] noiseMap = PerlinNoise.CreateNoiseMap(Seed, Size, Scale, Octaves, Persistence, Lacunarity);

        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                texture.SetPixel(x, y, Color.Lerp(Color.black, Color.white, CuttoffCurve.Evaluate(noiseMap[x, y])));
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();
        img.texture = texture;
        img.SetNativeSize();
    }*/

    private void OnValidate()
    {
        //CreateNoise();
    }
}
