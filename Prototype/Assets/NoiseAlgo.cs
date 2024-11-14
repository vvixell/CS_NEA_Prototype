using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseAlgo : MonoBehaviour
{

    public void Generate(int Seed, int MapSize, NoiseSettings settings, float CuttOff, bool FallOff)
    {
        DestroyImage();

        Texture2D texture = new Texture2D(MapSize, MapSize);

        float[,] noiseMap = PerlinNoise.CreateNoiseMap(Seed, new Vector2Int(MapSize, MapSize), settings, FallOff);

        for (int y = 0; y < MapSize; y++)
        {
            for (int x = 0; x < MapSize; x++)
            {
                texture.SetPixel(x, y, Color.Lerp(Color.black, Color.white, noiseMap[x, y] > CuttOff ? 1 : 0));
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        GameObject IMAGE = new GameObject("NoiseImage");
        IMAGE.transform.parent = transform;
        RawImage img = IMAGE.AddComponent<RawImage>();

        img.transform.localPosition = new Vector2(0, 0);
        img.texture = texture;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 450);
    }

    public void DestroyImage()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}
