using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen
{
    public class MapTexture : MonoBehaviour
    {
        public Renderer TextureRenderer;

        public void DrawNoiseMap(Cell[] Cells, int MapWidth, int MapHeight)
        {
            Texture2D Texture = new Texture2D(MapWidth, MapHeight);
            Color[] ColourMap = new Color[MapWidth * MapHeight];
            //Debug.Log("Elements" + Width * Height);

            for (int i = 0; i < MapWidth * MapHeight; i++)
            {
                ColourMap[i] = Color.white;

            }//paint it white
            int C = (MapHeight / 2) * MapWidth + (MapWidth / 2);//Center of 2d texture
            ColourMap[C] = Color.gray;
            ColourMap[0] = Color.green;
            ColourMap[5] = Color.red;


            //loop through each cell
            foreach (Cell cell in Cells)
            {
                // get the center position on the texture
                int YStart = (Mathf.RoundToInt(cell.Center.y) - cell.Height / 2);
                int XStart = (Mathf.RoundToInt(cell.Center.x) - cell.Width / 2);
               
                //loop through ever pixle of cell to draw it in
                for (int y = YStart; y < YStart + cell.Height; y++)
                {
                    for (int x = XStart; x < XStart + cell.Width; x++)
                    {
                        var PX = (y * MapWidth + x) + C; // position of 1 pixel of cell on texture
                        if (PX < 0 || PX > ColourMap.Length) // out of range 
                        {
                            continue;
                        }
                        else
                        {
                            //Debug.Log("index " + (PX).ToString());
                            ColourMap[PX] = Color.gray;
                        }
                    }
                }
            }

            Texture.SetPixels(ColourMap);
            Texture.Apply();
            Texture.filterMode = FilterMode.Point;
            Texture.wrapMode = TextureWrapMode.Clamp;
            TextureRenderer.sharedMaterial.SetTexture("_MainTex", Texture);
            TextureRenderer.transform.localScale = new Vector3(MapWidth, 1, MapHeight);
        }


        public void DrawRooms(Cell[] Cells, int MapWidth, int MapHeight, Cell[] SelectedRooms)
        {
            Texture2D Texture = new Texture2D(MapWidth, MapHeight);
            Color[] ColourMap = new Color[MapWidth * MapHeight];
            //Debug.Log("Elements" + Width * Height);

            for (int i = 0; i < MapWidth * MapHeight; i++)
            {
                ColourMap[i] = Color.white;

            }//paint it white
            int C = (MapHeight / 2) * MapWidth + (MapWidth / 2);//Center of 2d texture

            //loop through each cell
            foreach (Cell cell in Cells)
            {
                Color CellColour = Color.gray;

                foreach (var Room in SelectedRooms)
                {
                    if(cell == Room)
                    {
                        CellColour = Color.red;
                        break;
                    }
                }

                // get the center position on the texture
                int YStart = (Mathf.RoundToInt(cell.Center.y) - cell.Height / 2);
                int XStart = (Mathf.RoundToInt(cell.Center.x) - cell.Width / 2);

                //loop through ever pixle of cell to draw it in
                for (int y = YStart; y < YStart + cell.Height; y++)
                {
                    for (int x = XStart; x < XStart + cell.Width; x++)
                    {
                        var PX = (y * MapWidth + x) + C; // position of 1 pixel of cell on texture
                        if (PX < 0 || PX > ColourMap.Length) // out of range 
                        {
                            continue;
                        }
                        else
                        {
                            //Debug.Log("index " + (PX).ToString());
                            ColourMap[PX] = CellColour;
                        }
                    }
                }
            }

            Texture.SetPixels(ColourMap);
            Texture.Apply();
            Texture.filterMode = FilterMode.Point;
            Texture.wrapMode = TextureWrapMode.Clamp;
            TextureRenderer.sharedMaterial.SetTexture("_MainTex", Texture);
            TextureRenderer.transform.localScale = new Vector3(MapWidth, 1, MapHeight);
        }


        public GameObject[] CreateCells(Cell[] Cells)
        {
            GameObject[] gameObjects = new GameObject[Cells.Length];
            for (int i = 0; i < Cells.Length; i++)
            {
                gameObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                gameObjects[i].transform.position =  new Vector3(Cells[i].Center.x, 0, Cells[i].Center.y);
                gameObjects[i].transform.localScale = new Vector3((float)Cells[i].Width / 10, 0, (float)Cells[i].Height/ 10);
            }
            return gameObjects;
        }
    }
}
