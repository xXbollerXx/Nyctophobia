using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen
{
    [Serializable]
    public class Cell
    {
        public int Width;
        public int Height;
        public Vector2 Center;
        //public int XCenter;
       // public int YCenter;

        public Cell(float _Width, float _Height, Vector2 center)
        {
            Width = Mathf.RoundToInt(_Width);
            Height = Mathf.RoundToInt(_Height);
            Center = center;
            // Debug.Log("Spawned Width " + Width);
            //Debug.Log("Spawned Height " + Height);

            //Center = new Vector2(Width / 2, Height / 2);

            //XCenter = Mathf.RoundToInt(Center.x);
            //YCenter = Mathf.RoundToInt(Center.y);
            // Debug.Log("Spawned XCenter " + XCenter);
            //Debug.Log("Spawned YCenter " + YCenter);
        }
        public Cell(float width, float height, Vector3 center)
        {
            Width = Mathf.RoundToInt(width);
            Height = Mathf.RoundToInt(height);
            Center = new Vector2(center.x, center.z);

        }

        public void Move(int x, int y)
        {
            Center += new Vector2(x, y);
           // XCenter += x;
           // YCenter += y;
        }
/*
        public bool IsInsideBounds(Cell Other)
        {
            float OtherXRadius =  Other.Width / 2;
            float OtherYRadius =  Other.Height / 2;

            float XRadius =  Width / 2;
            float YRadius =  Height / 2;

            if (Other.Center.x - OtherXRadius <= Center.x + XRadius - 1&& Other.Center.x + OtherXRadius - 1 >= Center.x - XRadius && Other.Center.y - OtherYRadius <= Center.y + YRadius - 1&& Other.Center.y + OtherYRadius - 1 >= Center.y - YRadius)
            {
                return true;
            }

            return false;
        }*/

        public bool IsInsideBounds(Cell Other)
        {
            float OtherXRadius = Other.Width / 2;
            float OtherYRadius = Other.Height / 2;

            float XRadius = Width / 2;
            float YRadius = Height / 2;

            if (Other.Center.x - OtherXRadius <= Center.x + XRadius && Other.Center.x + OtherXRadius >= Center.x - XRadius && Other.Center.y - OtherYRadius <= Center.y + YRadius && Other.Center.y + OtherYRadius >= Center.y - YRadius)
            {
                return true;
            }

            return false;
        }

        public bool IsWithinBounds(Cell Other)
        {
            float OtherXRadius = Other.Width / 2;
            float OtherYRadius = Other.Height / 2;

            float XRadius = Width / 2;
            float YRadius = Height / 2;

            if (Other.Center.x - OtherXRadius <= Center.x + XRadius - 1 && Other.Center.x + OtherXRadius >= Center.x - XRadius + 1 && Other.Center.y - OtherYRadius <= Center.y + YRadius - 1 && Other.Center.y + OtherYRadius >= Center.y - YRadius + 1)
            {
                return true;
            }

            return false;
        }


        public bool IsInsideBounds(Vector2 point)
        {
            //TODO need to - 1 from top and right side
          
            float xRadius =  Width / 2;
            float yRadius =  Height / 2;
            return point.x <= Center.x + xRadius && point.x >= Center.x - xRadius  && point.y <= Center.y + yRadius && point.y >= Center.y - yRadius;
        }
        public bool IsOnBounds(Vector2 point)
        {
            float xRadius =  Width / 2;
            float yRadius =  Height / 2;
            return point.x == Center.x + xRadius  || point.x == Center.x - xRadius  || point.y == Center.y + yRadius  || point.y == Center.y - yRadius;
            //return !(point.x < Center.x + xRadius && point.x > Center.x - xRadius  && point.y < Center.y + yRadius && point.y > Center.y - yRadius);
        }
        public bool IsWithinBounds(Vector2 point)
        {
            float xRadius =  Width / 2 ;
            float yRadius =  Height / 2 ;
            return point.x <= Center.x + xRadius - 1 && point.x >= Center.x - xRadius + 1  && point.y <= Center.y + yRadius - 1 && point.y >= Center.y - yRadius + 1;
        }





            
    }
}
