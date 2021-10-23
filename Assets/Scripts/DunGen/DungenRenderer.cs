using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DunGen
{
    public class DungenRenderer : MonoBehaviour
    {
        static List<GameObject> Tiles = new List<GameObject>();
        public static void RenderRooms(Cell[] rooms, Edge[] edges, Cell[] cells)
        {
            DestroyDungeon();
            var tileMap = TileMap.Instance;
            int tileScale = 2; //in units !!! width and height !!!!
            List<Cell> hallways = new List<Cell>();
            CalculateHallways(edges, tileScale, tileMap, hallways);//render the paths between rooms
            RenderRooms(rooms, tileScale, tileMap, hallways.ToArray());//render the rooms
           

            Cell[] cellsToRender = GetCellsToRender(cells, hallways, tileScale, tileMap); // get all smaller rooms to render in path of hallways
            RenderRooms(cellsToRender, tileScale, tileMap,hallways.ToArray()); //render the cells
            var renderedCells = cellsToRender.Concat(rooms); //bad?

             RenderHallways(hallways.ToArray(), tileScale, tileMap, renderedCells.ToArray());

        }
        private static Cell[] GetCellsToRender(Cell[] cells, List<Cell> hallways, int tileScale, TileMap tileMap)
        {

            List<Cell> cellsToRender = new List<Cell>();
            //render cells in path 
            foreach (Cell cell in cells)
            {
                foreach (Cell hallway in hallways)
                {
                    if (cell.IsWithinBounds(hallway))
                    {
                        cellsToRender.Add(cell);
                        break;
                    }
                }
            }
            return cellsToRender.ToArray();
        }

        private static void CalculateHallways(Edge[] edges, int tileScale, TileMap tileMap, List<Cell> hallways)
        {
            foreach (Edge edge in edges)
            {
                //lengths of the hallways 
                float width = Mathf.Abs(edge.Vertex2.x - edge.Vertex1.x); // path 1
                float height = Mathf.Abs(edge.Vertex2.y - edge.Vertex1.y); // path 2
                // position of first room where paths are coming from
                Vector3 originalRoom = new Vector3(edge.Vertex1.x, 0, edge.Vertex1.y); 

                //get the direction of the edge 
                Vector3 direction = new Vector3(edge.Vertex2.x , 0, edge.Vertex2.y) - new Vector3(edge.Vertex1.x , 0, edge.Vertex1.y );
                direction.Normalize();

                //get direction to go out from 
                //go up/down first 
                if (Mathf.Abs(direction.x) < Mathf.Abs(direction.z)) 
                {
                    // round z to 1 or - 1
                    Vector3 tempDir = new Vector3(0, 0, Mathf.Sign(direction.z));
                    hallways.Add(new Cell(4, height + 4, originalRoom + (tempDir * height / 2)));

                    //build second path 
                    tempDir = new Vector3(Mathf.Sign(direction.x), 0, 0);
                    Vector3 temp = originalRoom + tempDir * width / 2;
                    // align with the destination room 
                    temp.z = edge.Vertex2.y; 
                    hallways.Add(new Cell(width + 4, 4, temp));

                }
                //go left/right first 
                else
                {
                    // round x to 1 or - 1
                    Vector3 tempDir = new Vector3(Mathf.Sign(direction.x), 0, 0);
                    hallways.Add(new Cell(width + 4, 4, originalRoom + tempDir * width / 2));

                    //build second path
                    tempDir = new Vector3(0, 0, Mathf.Sign(direction.z));
                    Vector3 temp = originalRoom + tempDir * height / 2;
                    // align with the destination room 
                    temp.x = edge.Vertex2.x;
                    hallways.Add(new Cell(4, height + 4, temp));
                }
            }
        }
        
        private static void RenderRooms(Cell[] cells, int tileScale, TileMap tileMap, Cell[] hallways)
        {
            foreach (Cell room in cells)
            {

                int roomWorldX = (int)room.Center.x;
                int roomWorldY = (int)room.Center.y;
                int width = room.Width / 2;
                int height = room.Height / 2;
                
                //place floors 
                for (int x = roomWorldX - width; x <= roomWorldX + width; x ++)
                {
                    for (int y = roomWorldY - height; y <= roomWorldY + height; y ++)
                    {
                        //if x y is inside of cell -- floor
                        //if x y is on edge -- wall 
                        //if walls are intersecting with hallways -- wall    -- maybe if the edge is parallel then floor??
                        //if edge is inside hallway then floor 
                        //this way hallways can be rendered in if they're not inside of cells 
                        if (room.IsOnBounds(new Vector2(x,y)) && !hallways.Any(hallway => hallway.IsWithinBounds(new Vector2(x,y))))
                        {
                            Tiles.Add(Instantiate(tileMap.Block, new Vector3(x, 0, y) * tileScale, Quaternion.identity));
                            continue;
                        }
                        Tiles.Add(Instantiate(tileMap.Floor, new Vector3(x, 0, y) * tileScale, Quaternion.identity));
                    }
                }
                
                //Tiles.Add(Instantiate(tileMap.Wall_Bottom, new Vector3(roomWorldX + width, 0, roomWorldY + height) * tileScale, Quaternion.identity));

                
            }
        }
        
        private static void RenderHallways(Cell[] hallways, int tileScale, TileMap tileMap, Cell[] otherRooms)
        {
            foreach (Cell room in hallways)
            {

                int roomWorldX = (int)room.Center.x;
                int roomWorldY = (int)room.Center.y;
                int width = room.Width / 2;
                int height = room.Height / 2;
                
                //place floors 
                for (int x = roomWorldX - width; x <= roomWorldX + width; x ++)
                {
                    for (int y = roomWorldY - height; y <= roomWorldY + height; y ++)
                    {
                        bool isoverlappingcell = false;
                        //is this vector inside of a cell?
                        foreach (Cell other in otherRooms)
                        {
                            if (other.IsInsideBounds(new Vector2(x, y)))
                            {
                                isoverlappingcell = true;
                                break;
                            }
                        }

                        bool isoverlappinghall = false;
                        //is this vector within a hallway?
                        foreach (Cell other in hallways)
                        {
                            if (other == room) continue; //skip me cos dont want to know if overlapping self
                            if (other.IsWithinBounds(new Vector2(x, y)))
                            {
                                isoverlappinghall = true;
                                break;
                            }
                        }

                        // if nothing is overlapping the hallway 
                        if (!isoverlappingcell )
                        {
                            //if the vector is on the edge of the current hallway but not within another hallway then place a wall 
                            if (room.IsOnBounds(new Vector2(x, y)) && !isoverlappinghall)
                            {
                                //add wall
                                Tiles.Add(Instantiate(tileMap.Block, new Vector3(x, 0, y) * tileScale, Quaternion.identity));

                            }
                            //add floor
                            Tiles.Add(Instantiate(tileMap.Floor, new Vector3(x, 0, y) * tileScale, Quaternion.identity));
                        }
                    }
                }
                
            }
        }

        public static void DestroyDungeon()
        {
            if (Tiles.Count <= 0) return;
            foreach (GameObject tile in Tiles)
            {
                DestroyImmediate(tile);
            }
            Tiles.Clear();
        }

   

    }
}
