using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int MapWidth, MapHeight;
    public float Scale, TileSpacing;
    [Range(0, 1)]
    public float TileThreshold;
    public bool AutoUpdate = false;
    GameObject[] Tiles = new GameObject[1];


    public void GenerateMap()
    {

        MapDisplay display = GetComponent<MapDisplay>();//attached to same gameobject. turns noise into textuure

        display.DrawNoiseMap(Noise.GenNoiseMap(MapWidth, MapHeight, Scale));// call the textre maker 






        TileGenerator TileGen = GetComponent<TileGenerator>();
        DeleteTiles();

        Tiles = TileGen.GenerateTiles(Noise.GenNoiseMap(MapWidth, MapHeight, Scale), TileThreshold, TileSpacing);

    }

    public void ClearTiles()
    {
        DeleteTiles();
    }

    private void DeleteTiles()
    {
        if (Tiles.Length > 0)
        {
            foreach (GameObject Tile in Tiles)
            {
                if (Tile)
                {
                    DestroyImmediate(Tile);
                }
            }
        }
    }

    private void OnValidate()
    {

        MapWidth = Mathf.Max(MapWidth, 1);
        MapHeight = Mathf.Max(MapHeight, 1);

    }
}
