using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenNoiseMap(int MapWidth, int MapHeight, float Scale)
    {
        //Scale = Mathf.Max(Scale, 0.00001f);

        if (Scale <= 0)
        {
            Scale = 0.0001f;
        }

        float[,] NoiseMap = new float[MapWidth, MapHeight];

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                float SampleX = x / Scale;
                float SampleY = y / Scale;

                float PerlinValue = Mathf.PerlinNoise(SampleX, SampleY);
                PerlinValue = PerlinValue > 0.5f ? 1 : 0;
                NoiseMap[x, y] = PerlinValue;
            }
        }
        return NoiseMap;
    }
}
