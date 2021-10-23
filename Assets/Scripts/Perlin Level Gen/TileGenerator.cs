using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject TileEmptyPrefab, TileWallPrefab, TileCornerPrefab;
    public TileMap tile;
    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<TileMap>();
    }

  
    public GameObject[] GenerateTiles(float[,] NoiseMap, float Threshold, float Spacing)
    {
        int Width = NoiseMap.GetLength(0);
        int Height = NoiseMap.GetLength(1);

        GameObject[] Tiles = new GameObject[Width * Height];
        int TileIndex = 0;

        for (int y = 1; y < Height - 1; y++)//easy fix
        {
            for (int x = 1; x < Width - 1; x++)
            {
                if(NoiseMap[x,y] >= Threshold)
                {
                    if (isTileThere(NoiseMap[x - 1, y], Threshold) && isTileThere(NoiseMap[x + 1, y], Threshold) && isTileThere(NoiseMap[x, y + 1], Threshold) && isTileThere(NoiseMap[x, y - 1], Threshold))
                    {
                        Tiles[TileIndex] = Instantiate(tile.Floor, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);
                        //continue;

                    }

                    if (isTileThere(NoiseMap[x - 1, y], Threshold) && !isTileThere(NoiseMap[x + 1, y], Threshold)) //floor to left
                    {
                        if(isTileThere(NoiseMap[x, y + 1], Threshold) && !isTileThere(NoiseMap[x, y - 1], Threshold)) // floor to top
                        {
                            //Bottom Right corner
                            Tiles[TileIndex] = Instantiate(tile.Corner_BottomRight, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }
                        else if (isTileThere(NoiseMap[x, y - 1], Threshold) && !isTileThere(NoiseMap[x, y + 1], Threshold)) // floor to bottom 
                        {
                            //Top Right corner
                            Tiles[TileIndex] = Instantiate(tile.Corner_TopRight, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }
                        else
                        {
                            //Right Wall
                            Tiles[TileIndex] = Instantiate(tile.Wall_Right, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }


                    }
                    else if (isTileThere(NoiseMap[x + 1, y], Threshold) && !isTileThere(NoiseMap[x - 1, y], Threshold)) //floor to Right
                    {
                        if (isTileThere(NoiseMap[x, y + 1], Threshold) && !isTileThere(NoiseMap[x, y - 1], Threshold)) // floor to top
                        {
                            //Bottom Left corner
                            Tiles[TileIndex] = Instantiate(tile.Corner_BottomLeft, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }
                        else if (isTileThere(NoiseMap[x, y - 1], Threshold) && !isTileThere(NoiseMap[x, y + 1], Threshold)) // floor to bottom 
                        {
                            //Top left corner
                            Tiles[TileIndex] = Instantiate(tile.Corner_TopLeft, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }
                        else
                        {
                            //left wall
                            Tiles[TileIndex] = Instantiate(tile.Wall_Left, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }
                        

                    }

                    if(isTileThere(NoiseMap[x - 1, y], Threshold) && isTileThere(NoiseMap[x + 1, y], Threshold)) //floor left and right
                    {
                        if(isTileThere(NoiseMap[x, y + 1], Threshold) && !isTileThere(NoiseMap[x, y - 1], Threshold)) // floor top
                        {
                            //bottom wall 
                            Tiles[TileIndex] = Instantiate(tile.Wall_Bottom, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }
                        else if(!isTileThere(NoiseMap[x, y + 1], Threshold) && isTileThere(NoiseMap[x, y - 1], Threshold)) //floor bottom 
                        {
                            //top wall
                            Tiles[TileIndex] = Instantiate(tile.Wall_Top, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                        }
                    }


                    /*//corner
                    if ( isTileThere(NoiseMap[x + 1, y], Threshold) && isTileThere(NoiseMap[x, y + 1], Threshold) && !isTileThere(NoiseMap[x - 1, y], Threshold) && !isTileThere(NoiseMap[x , y - 1], Threshold))
                    {
                        //bottom left corner
                        Tiles[TileIndex] = Instantiate(TileEmptyPrefab, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                    }
                    else if (isTileThere(NoiseMap[x - 1, y], Threshold) && isTileThere(NoiseMap[x, y + 1], Threshold) && !isTileThere(NoiseMap[x + 1, y], Threshold) && !isTileThere(NoiseMap[x, y - 1], Threshold))
                    {
                        //bottom right corner
                        Tiles[TileIndex] = Instantiate(TileCornerPrefab, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                    }
                    else if (isTileThere(NoiseMap[x + 1, y], Threshold) && isTileThere(NoiseMap[x, y - 1], Threshold) && !isTileThere(NoiseMap[x - 1, y], Threshold) && !isTileThere(NoiseMap[x, y + 1], Threshold))
                    {
                        //Top Left corner
                        Tiles[TileIndex] = Instantiate(TileCornerPrefab, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                    }
                    else if (isTileThere(NoiseMap[x - 1, y], Threshold) && isTileThere(NoiseMap[x, y - 1], Threshold) && !isTileThere(NoiseMap[x + 1, y], Threshold) && !isTileThere(NoiseMap[x, y + 1], Threshold))
                    {
                        //Top right corner
                        Tiles[TileIndex] = Instantiate(TileCornerPrefab, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);

                    }
                    else
                    {

                        Tiles[TileIndex] = Instantiate(TileEmptyPrefab, new Vector3(x * Spacing, 0, y * Spacing), Quaternion.identity);
                    }*/
                    TileIndex++; 
                }
            
            }
        }

        return Tiles;
    }

   bool isTileThere(float Value, float Threshold)
    {
        if(Value >= Threshold)
        {
            return true; 
        }
        return false;
    }
}
