using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DunGen
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Map")]
        public int MapWidth = 1;
        public int MapHeight = 1;

        [Header("Cell Spawning Settings")]
        public float CellMinSize = 1;
        public float CellMaxSize = 2, SpawnAreaRadius = 1;
        public int NumberOfCells = 50;
        public float Mean = 0, Deviation = 10;

        [Header("distrabution settings")]
        public int MaxCount = 50;
        public int DistrabutionM = 4;

        [Header("Big Rooms")]
        public float Threshold = 1.25f;

        [Header("Script settings")]
        public bool AutoUpdate = false;
        public bool SpawnPlanes = false;
        public bool ShowTriangulation = false;
        public bool RenderDungeon = false;
        public bool GenNavMesh = false;
        GameObject[] Planes = new GameObject[0];

        public static MapGenerator Instance;
   

        private Cell[] Cells;

        public void GenerateMap() //gets called by button 
        {
            Cell[] Rooms;

            do
            {
                Cells = CellGen.GenerateCells(CellMinSize, CellMaxSize, SpawnAreaRadius, NumberOfCells, Mean,  Deviation); //generates all the cells on the map in radius 
                Rooms = CellGen.GetRooms(Cells, Threshold);//get the Biggest cells NEED
            } while (Rooms.Length < 3); //keep gening if rooms is less than 3
            
            CellGen.DisperseCells(Cells, MaxCount); // move cells away from each other

            MapTexture mapTexture = GetComponent<MapTexture>();// gets ref to component NEED

            //mapTexture.DrawNoiseMap(Cells, MapWidth, MapHeight);//part of texture 
            mapTexture.DrawRooms(Cells, MapWidth, MapHeight, Rooms); //draw rooms as red NEED
            
            //---spawn planes as the rooms 
            if (SpawnPlanes)
            {
                ClearTiles(); // clear old planes before making new ones 
                Planes = mapTexture.CreateCells(Cells); // create the new planes
            }

            if (RenderDungeon)
            {
                DungenRenderer.RenderRooms(Rooms, MinimumSpanningTree.GenerateTree(Rooms, ShowTriangulation), Cells);
            }

            if (GenNavMesh)
            {
                GetComponent<NavMeshSurface>()?.BuildNavMesh();
                //spawn enemies 
                GetComponent<EnemySpawner>()?.SpawnEnemies();

            }

            SetupPlayer(Rooms[0].Center);
            Cells = null;
            Rooms = null;
            
        }

        public void ClearTiles()
        {
            foreach (GameObject item in Planes)
            {
                DestroyImmediate(item);
            }

        }

        private void Start()
        {
            if (!Instance)
            {
                Instance = this;

            }

        }

        void SetupPlayer(Vector2 location)
        {
            Vector3 newloc = Vector3.zero;
            int count = 0;
            do
            {
                Vector3 randomLocation = UnityEngine.Random.insideUnitSphere * 1000;
                if (NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, 100, 1))
                {
                    newloc = hit.position;
                    newloc.y = 0.01f;
                    PlayerMovement.Instance.transform.position = newloc;
                    return;
                }
                count++;
            } while (count < 1000);

            newloc = new Vector3(location.x, 0, location.y);
            newloc *= 2;
            newloc.y = 0.01f;

            PlayerMovement.Instance.transform.position = newloc;
        }

       public void NextLevel()
        {
            DungenRenderer.DestroyDungeon();
            GameManager.Instance.NextLevel();
        }
    }
}
