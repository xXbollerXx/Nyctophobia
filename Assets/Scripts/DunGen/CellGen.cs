using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen
{
    public static class CellGen
    {
       
        static float  NormalizedRandom(float MinValue, float MaxValue)
        {
            float u, v, S;

            do
            {
                u = 2.0f * UnityEngine.Random.value - 1.0f;
                v = 2.0f * UnityEngine.Random.value - 1.0f;
                S = u * u + v * v;
            }
            while (S >= 1.0f);

            // Standard Normal Distribution
            float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

            // Normal Distribution centered between the min and max value
            // and clamped following the "three-sigma rule"
            float mean = (MinValue + MaxValue) / 2.0f;
            float sigma = (MaxValue - mean) / 3.0f;
            return Mathf.Clamp(std * sigma + mean, MinValue, MaxValue);
        } //old Random number 

        static int RandomGaussian(float Mean, float Deviation)
        {
            float v1, v2, s;
            do
            {
                v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1.0f || s == 0f);

            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

            float Temp = v1 * s;

            return (int)(Mean + Temp * Deviation);
        }

        static float RandomGaussian(float MinValue, float MaxValue, float Mean, float Deviation)
        {
            float x;
            do
            {
                x = RandomGaussian(Mean, Deviation);
            } while (x < MinValue || x > MaxValue || x % 2 != 0);
            return x;
        }

        public static Cell[] GenerateCells(float MinValue, float MaxValue, float SpawnRadius, int NumberOfCells, float Mean, float Deviation)
        {
            Cell[] Rooms = new Cell[NumberOfCells];

            for (int i = 0; i < NumberOfCells; i++)
            {

                Vector2 temp = Random.insideUnitCircle * SpawnRadius; // get random point
                Vector2 center = new Vector2((int)temp.x, (int)temp.y);//make vector 2 be ints
                Cell cell = new Cell(RandomGaussian(MinValue, MaxValue, Mean, Deviation), RandomGaussian(MinValue, MaxValue, Mean, Deviation),center);
                Rooms[i] = cell;
            }
            
            return Rooms;
        }
        
        //return the biggest cells 
        public static Cell[] GetRooms(Cell[] Cells, float Threshold)
        {
            float WidthMean = 0, HeightMean = 0;

            foreach (Cell cell in Cells) // loop through Cells and get the biggest rooms  
            {
                WidthMean += cell.Width;
                HeightMean += cell.Height;
            }

            WidthMean /= Cells.Length;
            HeightMean /= Cells.Length;
            int RoomMinWidth = Mathf.RoundToInt(Threshold * WidthMean);
            int RoomMinHeight = Mathf.RoundToInt(Threshold * HeightMean);
            List<Cell> SelectedRooms = new List<Cell>();

            foreach (Cell cell in Cells) // loop through Cells and get the biggest rooms  
            {
                if(cell.Width >= RoomMinWidth && cell.Height >= RoomMinHeight)
                {
                    SelectedRooms.Add(cell);
                }
            }
            return SelectedRooms.ToArray();
        }
        
        //move all the cells away from each other 
        public static void DisperseCells(Cell[] cells, int maxCount)
        {

            bool cellInRadius;// dont stop loop until all cells are away from each other
            int count = 0; // stop loop if exceeded count 
            
            do
            {
                cellInRadius = false;
                MoveCells(cells, ref cellInRadius);

                // loop control logic
                count++;
                if (count < maxCount) continue;
                cellInRadius = false;
                Debug.LogWarning("loop exceeded Count");

            } while (cellInRadius);

 
        }
        
        private static void MoveCells(Cell[] cells, ref bool cellInRadius)
        {
            foreach (Cell cell in cells) //do this for every cell
            {
                Vector2 direction = Vector2.zero;
                foreach (Cell other in cells) // loop through every cell again 
                {
                    if (cell == other) continue; // skip myself
                    if (!cell.IsInsideBounds(other)) continue; //skip if nothing inside bounds
                    
                    Vector2 Temp = (cell.Center - other.Center).normalized; //get direction from other to me
                    direction += Temp; //get direction away from everything in my bounds
                    cellInRadius = true; // there was other in bounds so loop through everything again
                }
                cell.Center += new Vector2(Mathf.RoundToInt(direction.x),Mathf.RoundToInt(direction.y)); // move cell away from other cells
            }
        }
    }
}
